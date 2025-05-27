describe('User Registration and Login', () => {

    const uniqueUser = `testuser_${new Date().getTime()}`

    it('should allow a user to register', () => {

        cy.intercept('POST', 'https://localhost:7209/api/Account/register').as('registerRequest')

        cy.visit('/register')
        cy.wait(1000);
        cy.get('#username').type(uniqueUser)
        cy.wait(1000);
        cy.get('#email').type('testuser@example.com')
        cy.wait(1000);
        cy.get('#password').type('Testpassword123!')
        cy.wait(1000);
        cy.get('#confirmPassword').type('Testpassword123!')
        cy.wait(1000);
        cy.get('#register-button').click()
        cy.wait(1000);

        cy.wait('@registerRequest').then((interception) => { expect(interception.response.statusCode).to.equal(200) })
        cy.wait(1000);
    })

    it('should allow a user to login', () => {

        cy.intercept('POST', 'https://localhost:7209/api/Account/login').as('loginRequest')

        cy.visit('/login')
        cy.get('#username').type(uniqueUser)
        cy.wait(1000);
        cy.get('#password').type('Testpassword123!')
        cy.wait(1000);
        cy.get('#login-button').click()
        cy.wait(1000);

        cy.wait('@loginRequest').then((interception) => { expect(interception.response.statusCode).to.equal(200) })
        cy.wait(1000);
    })
})
