describe('Create Staff Profile Modal', () => {
    beforeEach(() => {
      // Navega até a página onde o componente está presente
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
      // Intercepta a chamada para a API
      cy.intercept('POST', '/api/staffs', {
        statusCode: 201,
        body: { success: true },
      }).as('createStaff');
  
      // Abre o modal
      cy.contains('Create Staff Profile').click();
  
      // Preenche o formulário
      cy.get<HTMLInputElement>('#firstName').type('John');
      cy.get<HTMLInputElement>('#lastName').type('Doe');
      cy.get<HTMLInputElement>('#fullName').type('John Doe');
      cy.get<HTMLInputElement>('#email').type('johndoe@example.com');
      cy.get<HTMLInputElement>('#phoneNumber').type('912345678');
      cy.get<HTMLInputElement>('#licenseNumber').type('D123456');
      cy.get<HTMLInputElement>('#userEmail').type('johndoe@example.com');
  
      // Seleciona uma especialização no dropdown
      cy.contains('label', 'Specialization')
        .parent()
        .find('p-dropdown')
        .click(); // Abre o dropdown

      // Seleciona a opção "Admin" na lista de opções visível
      cy.get('.p-dropdown-item')
        .contains('Admin')
        .click(); // Seleciona a opção
  
      // Salva o formulário
      cy.contains('Save').click();
  
      // Aguarda a chamada à API ser concluída
      cy.wait('@createStaff');
  
      // Verifica se o modal foi fechado e a mensagem de sucesso exibida
      cy.get('p-dialog').should('not.be.visible');
      cy.get('.p-toast').should('contain', 'Staff profile create successfully');
    });

    it('should close dialog and reset form when clicking Cancel', () => {
      cy.get('button').contains('Create Staff Profile').click();
      cy.get('#firstName').type('John');
      cy.get('#lastName').type('Doe');
      cy.get('button').contains('Cancel').click();
      cy.get('.p-dialog').should('not.exist');
      
      // Reopen dialog and check if form was reset
      cy.get('button').contains('Create Staff Profile').click();
      cy.get('#firstName').should('have.value', '');
      cy.get('#lastName').should('have.value', '');
  
      // Verify we're still on the staffs page
      cy.url().should('eq', 'http://localhost:4200/admin/staffs');
    });

    it('should maintain URL after dialog interactions', () => {
      // Check initial URL
      cy.url().should('eq', 'http://localhost:4200/admin/staffs');
      
      // Open dialog
      cy.get('button').contains('Create Staff Profile').click();
      cy.url().should('eq', 'http://localhost:4200/admin/staffs');
      
      // Close dialog
      cy.get('button').contains('Cancel').click();
      cy.url().should('eq', 'http://localhost:4200/admin/staffs');
    });
  });