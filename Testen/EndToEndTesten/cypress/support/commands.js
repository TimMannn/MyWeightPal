// ***********************************************
// This example commands.js shows you how to
// create various custom commands and overwrite
// existing commands.
//
// For more comprehensive examples of custom
// commands please read more here:
// https://on.cypress.io/custom-commands
// ***********************************************
//
//
// -- This is a parent command --
// Cypress.Commands.add('login', (email, password) => { ... })
//
//
// -- This is a child command --
// Cypress.Commands.add('drag', { prevSubject: 'element'}, (subject, options) => { ... })
//
//
// -- This is a dual command --
// Cypress.Commands.add('dismiss', { prevSubject: 'optional'}, (subject, options) => { ... })
//
//
// -- This will overwrite an existing command --
// Cypress.Commands.overwrite('visit', (originalFn, url, options) => { ... })

Cypress.Commands.add('login', (userName, password, rememberMe) => {
    // Login via API en stel token in localStorage
    cy.request('POST', 'https://localhost:7209/api/Account/login', {
        userName: 'CypressTestAccount', // Vervang dit door een valide gebruikersnaam
        password: 'CypressTestAccount123!',    // Vervang dit door een geldig wachtwoord
        rememberMe: true,
    }).then((response) => {
        expect(response.status).to.eq(200);
        const token = response.body.token;
        window.localStorage.setItem('token', token);
    });
});