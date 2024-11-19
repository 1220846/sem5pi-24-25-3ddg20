describe('Modal Create User Patient', () => {
  beforeEach(() => {
    cy.visit('/home'); // Atualize para a rota correta
    cy.get('.signup-link').should('be.visible'); // Garante que o link está visível
  });

  it('should open the modal when clicking Sign Up', () => {
    cy.get('.signup-link').click();
    cy.get('.p-dialog-header').should('contain', 'Create Account'); // Valida abertura do modal
  });

  it('should show validation errors for empty fields', () => {
    cy.get('.signup-link').click();
  
    // Clica no botão "Create Account" sem preencher os campos
    cy.get('.p-button-label').contains('Create Account').click();
  
    // Verifica se a mensagem de Warning aparece no Toast
    cy.get('.p-toast-message-content').should('be.visible').and('contain', 'Invalid Data in forms!');
  });
  

  it('should validate the email format', () => {
    cy.get('.signup-link').click();

    // Digita um e-mail inválido
    cy.get('input[formcontrolname="email"]').type('invalid-email').blur();
    cy.get('small.p-error').should('contain', 'Valid email is required.');

    // Digita um e-mail válido
    cy.get('input[formcontrolname="email"]').clear().type('valid@example.com').blur();
    cy.get('small.p-error').should('not.exist'); // Não deve mostrar mensagem de erro
  });

  it('should validate password strength', () => {
    cy.get('.signup-link').click();

    // Digita uma senha fraca
    cy.get('input[formcontrolname="password"]').type('weak').blur();
    cy.get('small.p-error').should(
      'contain',
      'Password must be at least 8 characters, include an uppercase letter, a number, and a special character.'
    );

    // Digita uma senha forte
    cy.get('input[formcontrolname="password"]').clear().type('StrongPass123!').blur();
    cy.get('small.p-error').should('not.exist'); // Mensagem de erro deve desaparecer
  });

  it('should validate password match', () => {
    cy.get('.signup-link').click();

    // Digita senhas diferentes
    cy.get('input[formcontrolname="password"]').type('StrongPass123!');
    cy.get('input[formcontrolname="repeatPassword"]').type('DifferentPass123!').blur();
    cy.get('small.p-error').should('contain', 'Passwords do not match.');

    // Corrige a senha repetida
    cy.get('input[formcontrolname="repeatPassword"]').clear().type('StrongPass123!').blur();
    cy.get('small.p-error').should('not.exist'); // Erro deve desaparecer
  });

  it('should create an account successfully', () => {
    cy.intercept('POST', '**/api/users/patients', { statusCode: 201 }).as('createAccount');

    cy.get('.signup-link').click();

    // Preenche os dados válidos
    cy.get('input[formcontrolname="email"]').type('test@example.com');
    cy.get('input[formcontrolname="password"]').type('StrongPass123!');
    cy.get('input[formcontrolname="repeatPassword"]').type('StrongPass123!');

    // Clica em "Create Account"
    cy.get('.p-button-label').contains('Create Account').click();

    // Valida a chamada para o backend
    cy.wait('@createAccount').then((interception) => {
      expect(interception.response?.statusCode).to.eq(201);
    });

    // Verifica a mensagem de sucesso
    cy.get('.p-toast-message-content').should('contain', 'Account registered successfully');
    cy.get('.p-dialog').should('not.exist'); // Modal deve fechar
  });

  it('should show error message on API failure', () => {
    cy.intercept('POST', '**/api/users/patients', {
      statusCode: 400,
      body: { message: 'An error occurred' },
    }).as('createAccountError');

    cy.get('.signup-link').click();

    // Preenche os dados válidos
    cy.get('input[formcontrolname="email"]').type('test@example.com');
    cy.get('input[formcontrolname="password"]').type('StrongPass123!');
    cy.get('input[formcontrolname="repeatPassword"]').type('StrongPass123!');

    // Clica em "Create Account"
    cy.get('.p-button-label').contains('Create Account').click();

    // Aguarda a resposta de erro
    cy.wait('@createAccountError');

    // Verifica a mensagem de erro
    cy.get('.p-toast-message-content').should('contain', 'An error occurred');
    cy.get('.p-dialog').should('be.visible'); // Modal deve continuar aberto
  });
});
