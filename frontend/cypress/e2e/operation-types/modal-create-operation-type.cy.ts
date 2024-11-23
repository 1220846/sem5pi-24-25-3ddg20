describe('Modal Create Operation Type - Simulating Auth via LocalStorage', () => {
  beforeEach(() => {
    const fakeLoginResponse = {
      loginToken: 'fake-jwt-token',
      roles: ['Admin'], 
    };
    
    window.localStorage.setItem('accessToken', fakeLoginResponse.loginToken);
    window.localStorage.setItem('roles', JSON.stringify(fakeLoginResponse.roles));

    // Interceptar APIs usadas pela página
    cy.intercept('GET', '**/api/specializations', {
      statusCode: 200,
      body: [
        { id: 1, name: 'Anaesthetist' },
        { id: 2, name: 'Circulating Nurse' },
        { id: 3, name: 'Orthopedics' }
      ]
    }).as('getSpecializations');

    cy.intercept('GET', '**/api/operationtypes/filter', {
      statusCode: 200,
      body: []
    }).as('getOperationTypes');

    // Visitar a página diretamente
    cy.visit('/admin/operation-types');

    // Verificar se a rota foi carregada corretamente
    cy.url().should('include', '/admin/operation-types');
    cy.get('p-button').should('be.visible');
  });

  it('should open modal when clicking add button', () => {

    cy.get('p-button').contains('Add Operation Type').should('be.visible').click();
    
    // Verify if modal is open
    cy.get('.p-dialog-content').should('be.visible');
    cy.get('.p-dialog-title').should('contain', 'Add Operation Type');
  });

  it('should show validation errors when trying to save empty form', () => {

    cy.get('p-button').contains('Add Operation Type').should('be.visible').click();
    cy.get('.p-dialog-content').should('be.visible');
    
    // Click save without values in form
    cy.get('p-button').contains('Save').click();

    // Verify message error
    cy.get('[formcontrolname="name"]').click().blur();
    cy.get('small.p-error').should('exist').and('be.visible').and('contain', 'Is required');

    // Verify multiple validation errors
    cy.get('small.p-error').should('have.length.at.least', 1);
  });

  it('should add specialization when selected from dropdown', () => {

    cy.get('p-button').contains('Add Operation Type').should('be.visible').click();
    cy.get('.p-dialog-content').should('be.visible');
    
    // Wait for specializations to load
    cy.wait('@getSpecializations');

    cy.get('p-dropdown').click();
    cy.get('.p-dropdown-panel').should('be.visible');

    // Add Specialization 
    cy.get('.p-dropdown-item').contains('Anaesthetist').click();
    cy.get('table').should('be.visible');
    cy.get('table tbody tr').should('have.length', 1);
    cy.get('table tbody tr td').first().should('contain', 'Anaesthetist');
  });

  it('should remove specialization when clicking remove button', () => {

    cy.get('p-button').contains('Add Operation Type').should('be.visible').click();
    cy.get('.p-dialog-content').should('be.visible');
    
    // Wait for specializations to load
    cy.wait('@getSpecializations');

    cy.get('p-dropdown').click();
    cy.get('.p-dropdown-panel').should('be.visible');

    // Add Specialization 
    cy.get('.p-dropdown-item').contains('Anaesthetist').click();
    cy.get('table tbody tr').should('have.length', 1);

    // Remove specialization
    cy.get('table tbody tr').find('p-button[severity="danger"]').click();
    cy.get('table tbody tr').should('not.exist');
  });

  it('should successfully create operation type with valid data', () => {

    // Stub the create operation type endpoint
    cy.intercept('POST', '**/api/operationtypes', (req) => {
      req.reply({
        statusCode: 201,
        body: {
          id: 1,
          name: 'ACL Reconstruction Surgery',
          estimatedDuration: 180,
          surgeryTime: 120,
          anesthesiaTime: 30,
          cleaningTime: 30,
          staffSpecializations: [
            { specializationId: 1, numberOfStaff: 2 }
          ]
        }
      });
    }).as('createOperationType');
  
    // Open Dialog
    cy.get('p-button').contains('Add Operation Type').should('be.visible').click();
    cy.get('.p-dialog-content').should('be.visible');

    // Add values in forms
    cy.get('[formcontrolname="name"]').clear().type('ACL Reconstruction Surgery'); 
    cy.get('[formcontrolname="surgeryTime"] input').clear().type('120');
    cy.get('[formcontrolname="anesthesiaTime"] input').clear().type('30');
    cy.get('[formcontrolname="cleaningTime"] input').clear().type('30');
    cy.get('p-dropdown').click();
    cy.get('.p-dropdown-panel').should('be.visible'); 
    cy.get('.p-dropdown-item').contains('Anaesthetist').click();
    cy.get('table tbody tr p-inputNumber input').clear().type('2'); 
  
    cy.get('p-button').contains('Save').click();
  
    cy.wait('@createOperationType') 
    .then((interception) => {
      expect(interception).to.have.property('response');
      expect(interception.response).to.have.property('statusCode').and.eq(201); 
      expect(interception.response?.body.name).to.eq('ACL Reconstruction Surgery');
    });

    // Verify toast message 
    cy.get('.p-toast-message-content').should('be.visible')
      .and('contain', 'Operation type added successfully');
    
    // Verify if the modal was closed
    cy.get('.p-dialog-content').should('not.exist');
  });  

  it('should clear form when canceling', () => {
    cy.get('p-button').contains('Add Operation Type').should('be.visible').click();
    cy.get('.p-dialog-content').should('be.visible');
    
    // Add values in forms
    cy.get('[formcontrolname="name"]').clear().type('ACL Reconstruction Surgery'); 
    cy.get('[formcontrolname="surgeryTime"] input').clear().type('120');
    cy.get('[formcontrolname="anesthesiaTime"] input').clear().type('30');
    cy.get('[formcontrolname="cleaningTime"] input').clear().type('30');
    cy.get('p-dropdown').click();
    cy.get('.p-dropdown-panel').should('be.visible'); 
    cy.get('.p-dropdown-item').contains('Anaesthetist').click();
    cy.get('table tbody tr p-inputNumber input').clear().type('2'); 

    cy.get('p-button').contains('Cancel').click();

    // Verify if forms are cleared
    cy.get('p-button').contains('Add Operation Type').click();
    cy.get('[formcontrolname="name"]').should('have.value', '');
    cy.get('[formcontrolname="surgeryTime"] input').should('have.value', '');
  });

  it('should show error message on API failure', () => {

    // Stub API error
    cy.intercept('POST', '**/api/operationtypes', {
      statusCode: 400,
      body: { message: 'An expected error occurred!' }
    }).as('createOperationTypeError');
  
    cy.get('p-button').contains('Add Operation Type').should('be.visible').click();
    cy.get('.p-dialog-content').should('be.visible');
    
    // Add values in forms
    cy.get('[formcontrolname="name"]').clear().type('ACL Reconstruction Surgery'); 
    cy.get('[formcontrolname="surgeryTime"] input').clear().type('120');
    cy.get('[formcontrolname="anesthesiaTime"] input').clear().type('30');
    cy.get('[formcontrolname="cleaningTime"] input').clear().type('30');
    cy.get('p-dropdown').click();
    cy.get('.p-dropdown-panel').should('be.visible'); 
    cy.get('.p-dropdown-item').contains('Anaesthetist').click();
    cy.get('table tbody tr p-inputNumber input').clear().type('2'); 
  
    cy.get('p-button').contains('Save').click();

    cy.wait('@createOperationTypeError').then((interception) => {
      expect(interception.response?.statusCode).to.eq(400);
      expect(interception.response?.body.message).to.eq('An expected error occurred!');
    });
  
    // Verify toast message 
    cy.get('.p-toast', { timeout: 10000 }).should('exist'); 
    cy.get('.p-toast-message-content', { timeout: 10000 })
      .should('be.visible')
      .and('contain', 'An expected error occurred!');
    
    cy.get('.p-dialog-content').should('be.visible');
  });
  
});