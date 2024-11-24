import { timeout } from "rxjs";

describe('Operation Request Modal Tests', () => {
  beforeEach(() => {
    const fakeLoginResponse = {
      loginToken: 'fake-jwt-token',
      roles: ['Doctor'],
    };

    window.localStorage.setItem('accessToken', fakeLoginResponse.loginToken);
    window.localStorage.setItem('roles', JSON.stringify(fakeLoginResponse.roles));

    // Intercept API calls
    cy.intercept('GET', '**/api/operationtypes', {
      statusCode: 200,
      body: [
        { id: 1, name: 'ACL Reconstruction Surgery' },
        { id: 2, name: 'Knee Arthroscopy' }
      ]
    }).as('getOperationTypes');

    cy.intercept('GET', '**/api/patients', {
      statusCode: 200,
      body: [
        { id: 'MRN001', name: 'John Doe' },
        { id: 'MRN002', name: 'Jane Smith' }
      ]
    }).as('getPatients');

    cy.visit('/doctor/operation-requests');
    cy.url().should('include', '/doctor/operation-requests');
  });

  it('should open modal when clicking Add Operation Request button', () => {
    cy.get('p-button').contains('Add Operation Request').click({force:true});
    cy.get('.p-dialog-title').should('contain', 'Add Operation Request');
  });

  it('should show validation messages when trying to save empty form', () => {
    cy.get('p-button').contains('Add Operation Request').click({force:true});
    cy.get('p-button').contains('Save').click({force:true});


    cy.get('.p-toast-message-content')
      .and('contain', 'Form is invalid. Please fill all required fields.');
  });

  it('should populate dropdowns with API data', () => {
    cy.get('p-button').contains('Add Operation Request').click({force:true});

    // Check Operation Types dropdown
    cy.get('[formcontrolname="operationType"]').click();
    cy.get('.p-dropdown-item').should('have.length', 2);
    cy.get('.p-dropdown-item').should('contain', 'ACL Reconstruction Surgery');
    cy.get('.p-dropdown-item').should('contain', 'Knee Arthroscopy');
    cy.get('[formcontrolname="operationType"]').click();


    // Check Patients dropdown
    cy.get('[formcontrolname="patient"]').click();
    cy.get('.p-dropdown-item').should('contain', 'MRN001');
    cy.get('.p-dropdown-item').should('contain', 'MRN002');
    cy.get('[formcontrolname="patient"]').click();

    cy.get('[formcontrolname="deadline"] input').type('15/12/2024');

    // Check Priority dropdown
    cy.get('[formcontrolname="selectedPriority"]').click();
    cy.get('.p-dropdown-item').should('contain', 'Elective');
    cy.get('.p-dropdown-item').should('contain', 'Urgent');
    cy.get('.p-dropdown-item').should('contain', 'Emergency');

  });

  it('should filter operation types using search', () => {
    cy.get('p-button').contains('Add Operation Request').click();
    cy.get('[formcontrolname="operationType"]').click();

    // Type in filter
    cy.get('.p-dropdown-filter').type('ACL');

    // Check filtered results
    cy.get('.p-dropdown-item').should('have.length', 1);
    cy.get('.p-dropdown-item').should('contain', 'ACL Reconstruction Surgery');
  });

});
