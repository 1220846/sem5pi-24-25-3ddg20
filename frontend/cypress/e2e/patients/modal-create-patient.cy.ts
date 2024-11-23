describe('Modal Create Patient', () => {

  const fakeLoginResponse = {
    loginToken: 'fake-jwt-token',
    roles: ['Admin'], 
  };

  window.localStorage.setItem('accessToken', fakeLoginResponse.loginToken);
  window.localStorage.setItem('roles', JSON.stringify(fakeLoginResponse.roles));

    beforeEach(() => {
      // Stub o serviço de pacientes
      cy.intercept('GET', '**/api/patients', {
        statusCode: 200,
        body: []
      }).as('getPatients');
  
      cy.visit('/admin/patients');
      cy.get('p-button').should('be.visible');
    });
  
    it('should open modal when clicking add button', () => {
      cy.get('p-button').contains('Add Patient').should('be.visible').click();
  
      // Verifique se o modal abriu
      cy.get('.p-dialog-content').should('be.visible');
      cy.get('.p-dialog-title').should('contain', 'Add Patient');
    });
  
    it('should show validation errors when trying to save empty form', () => {
      cy.get('p-button').contains('Add Patient').should('be.visible').click();
      cy.get('.p-dialog-content').should('be.visible');
  
      // Clique em salvar sem preencher o formulário
      cy.get('p-button').contains('Save').click();
  
      // Verifique mensagens de erro
      cy.get('[formcontrolname="firstName"]').click().blur();
      cy.get('small.p-error').should('exist').and('be.visible').and('contain', 'Insert First Name');
      cy.get('[formcontrolname="lastName"]').click().blur();
      cy.get('small.p-error').should('exist').and('be.visible').and('contain', 'Insert Last Name');
  
      // Valida múltiplos erros
      cy.get('small.p-error').should('have.length.at.least', 2);
    });
  
    it('should successfully create patient with valid data', () => {
      // Stub a API para criar paciente
      cy.intercept('POST', '**/api/patients', (req) => {
        req.reply({
          statusCode: 201,
          body: {
            id: 1,
            firstName: 'John',
            lastName: 'Doe',
            fullName: 'John Doe',
            dateOfBirth: '01/01/1990',
            gender: 'Male',
            phoneNumber: '912345678',
            emergencyContact: '912345679',
            email: 'john.doe@example.com',
            address: '123 Main St',
            postalCode: '1234-567'
          }
        });
      }).as('createPatient');
  
      // Abra o modal
      cy.get('p-button').contains('Add Patient').should('be.visible').click();
      cy.get('.p-dialog-content').should('be.visible');
  
      // Preencha o formulário
      cy.get('[formcontrolname="firstName"]').type('John');
      cy.get('[formcontrolname="lastName"]').type('Doe');
      cy.get('[formcontrolname="fullName"]').type('John Doe');
      cy.get('[formcontrolname="dateOfBirth"] input').type('01/01/1990');
      cy.get('p-dropdown#gender').click();
      cy.get('.p-dropdown-item').contains('Male').click();
      cy.get('[formcontrolname="phoneNumber"]').type('912345678');
      cy.get('[formcontrolname="emergencyContact"]').type('912345679');
      cy.get('[formcontrolname="email"]').type('john.doe@example.com');
      cy.get('[formcontrolname="address"]').type('123 Main St');
      cy.get('[formcontrolname="postalCode"]').type('1234-567');
  
      // Clique em salvar
      cy.get('p-button').contains('Save').click();
  
      // Verifique a resposta do stub
      cy.wait('@createPatient').then((interception) => {
        expect(interception).to.have.property('response');
        expect(interception.response?.statusCode).to.eq(201);
        expect(interception.response?.body.firstName).to.eq('John');
      });
  
  
      // Verifique se o modal foi fechado
      cy.get('.p-dialog-content').should('not.exist');
    });
  
    it('should clear form when canceling', () => {
      cy.get('p-button').contains('Add Patient').should('be.visible').click();
      cy.get('.p-dialog-content').should('be.visible');
  
      // Preencha alguns campos
      cy.get('[formcontrolname="firstName"]').type('John');
      cy.get('[formcontrolname="lastName"]').type('Doe');
      cy.get('[formcontrolname="phoneNumber"]').type('912345678');
  
      // Cancele
      cy.get('p-button').contains('Cancel').click();
  
      // Reabra e verifique se o formulário foi limpo
      cy.get('p-button').contains('Add Patient').click();
      cy.get('[formcontrolname="firstName"]').should('have.value', '');
      cy.get('[formcontrolname="lastName"]').should('have.value', '');
      cy.get('[formcontrolname="phoneNumber"]').should('have.value', '');
    });
  
    it('should show error message on API failure', () => {
      // Stub para erro de API
      cy.intercept('POST', '**/api/patients', {
        statusCode: 400,
        body: { message: 'An expected error occurred!' }
      }).as('createPatientError');
  
      cy.get('p-button').contains('Add Patient').should('be.visible').click();
      cy.get('.p-dialog-content').should('be.visible');
  
      // Preencha o formulário
      cy.get('[formcontrolname="firstName"]').type('John');
      cy.get('[formcontrolname="lastName"]').type('Doe');
      cy.get('[formcontrolname="fullName"]').type('John Doe');
      cy.get('[formcontrolname="phoneNumber"]').type('912345678');
  
      // Clique em salvar
      cy.get('p-button').contains('Save').click();
  
      // Verifique se o modal ainda está aberto
      cy.get('.p-dialog-content').should('be.visible');
    });
  });
  