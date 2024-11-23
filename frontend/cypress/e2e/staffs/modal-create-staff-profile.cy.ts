describe('Create Staff Profile Modal', () => {
    beforeEach(() => {
      const fakeLoginResponse = {
        loginToken: 'fake-jwt-token',
        roles: ['Admin'], 
      };
  
      window.localStorage.setItem('accessToken', fakeLoginResponse.loginToken);
      window.localStorage.setItem('roles', JSON.stringify(fakeLoginResponse.roles));

      cy.visit('http://localhost:4200/admin/staffs');
    });
  
    
    it('should load the staffs page correctly', () => {
        cy.url().should('eq', 'http://localhost:4200/admin/staffs');
        cy.get('button').contains('Create Staff Profile').should('be.visible');
      });

    it('should load modal when clicking the button', () => {
        cy.get('button').contains('Create Staff Profile').click();
        cy.get('.p-dialog').should('be.visible');
        cy.get('.p-dialog-header').should('contain', 'Create Staff Profile');
    });
  
    it('should validate if fields are filled', () => {
      cy.contains('Create Staff Profile').click();
  
      cy.get<HTMLInputElement>('#firstName').click();
      cy.get<HTMLInputElement>('#lastName').click();
      cy.get<HTMLInputElement>('#fullName').click();
      cy.get<HTMLInputElement>('#email').click();
      cy.get<HTMLInputElement>('#phoneNumber').click();
      cy.get<HTMLInputElement>('#licenseNumber').click();
      cy.get<HTMLInputElement>('#userEmail').click();

      cy.contains('Save').click();
  
      cy.get('.p-error').should('have.length', 7);
    });
  
    it('should successfully fill form', () => {
      cy.intercept('POST', '/api/staffs', {
        statusCode: 201,
        body: { success: true },
      }).as('createStaff');
  
      cy.contains('Create Staff Profile').click();
  
      cy.get<HTMLInputElement>('#firstName').type('John');
      cy.get<HTMLInputElement>('#lastName').type('Doe');
      cy.get<HTMLInputElement>('#fullName').type('John Doe');
      cy.get<HTMLInputElement>('#email').type('johndoe@example.com');
      cy.get<HTMLInputElement>('#phoneNumber').type('912345678');
      cy.get<HTMLInputElement>('#licenseNumber').type('D123456');
      cy.get<HTMLInputElement>('#userEmail').type('johndoe@example.com');
  
      cy.contains('label', 'Specialization')
        .parent()
        .find('p-dropdown')
        .click();

      cy.get('.p-dropdown-item')
        .contains('Admin')
        .click();
  
      cy.contains('Save').click();
  
      cy.wait('@createStaff');
  
      cy.get('p-dialog').should('not.be.visible');
      cy.get('.p-toast').should('contain', 'Staff profile create successfully');
    });

    it('should close dialog and reset form when clicking Cancel', () => {
      cy.get('button').contains('Create Staff Profile').click();
      cy.get('#firstName').type('John');
      cy.get('#lastName').type('Doe');
      cy.get('button').contains('Cancel').click();
      cy.get('.p-dialog').should('not.exist');
      
      cy.get('button').contains('Create Staff Profile').click();
      cy.get('#firstName').should('have.value', '');
      cy.get('#lastName').should('have.value', '');
  
      cy.url().should('eq', 'http://localhost:4200/admin/staffs');
    });

    it('should maintain URL after dialog interactions', () => {
      cy.url().should('eq', 'http://localhost:4200/admin/staffs');
      
      cy.get('button').contains('Create Staff Profile').click();
      cy.url().should('eq', 'http://localhost:4200/admin/staffs');
      
      cy.get('button').contains('Cancel').click();
      cy.url().should('eq', 'http://localhost:4200/admin/staffs');
    });
  });