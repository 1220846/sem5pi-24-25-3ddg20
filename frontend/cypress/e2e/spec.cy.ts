describe('My First Test', () => {
  it('Visits the initial project page', () => {
    cy.visit('/home');

    cy.get('img.logo').should('be.visible');

    cy.get('p-card.header').should('be.visible');
  });
});
    
