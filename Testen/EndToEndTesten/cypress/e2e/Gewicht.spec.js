describe('Quick login', () => {
    beforeEach(() => {
        cy.fixture('user').then((user) => {
            cy.login(user.username, user.password, user.rememberme);
        });
    });

    it('Should add a new gewicht', () => {
        cy.visit('/Gewicht');
        cy.wait(1000);
        cy.get('#GewichtToevoegenButton').click();
        cy.wait(1000);
        
        cy.get('input[placeholder="Voer je gewicht in."]').type(80);
        cy.wait(1000);
        cy.contains('Opslaan').click();

        cy.contains('Gewicht toegevoegd!').should('be.visible');
        cy.wait(1000);
    });

    it('Should add a new doelgewicht', () => {
        cy.visit('/Gewicht');
        cy.wait(1000);
        cy.get('#DoelgewichtToevoegenButton').click();
        cy.wait(1000);

        cy.get('input[placeholder="Voer je doelgewicht in."]').type(85);
        cy.wait(1000);
        cy.contains('Opslaan').click();

        cy.contains('Doelgewicht toegevoegd!').should('be.visible');
        cy.wait(1000);
    });

    it('Should edit a gewicht', () => {
        cy.visit('/Gewicht');
        cy.wait(1000);
        cy.get('#GewichtToevoegenButton').click();
        cy.wait(1000);

        cy.get('input[placeholder="Voeg je nieuwe gewicht in."]')
            .clear()
            .type(82);
        cy.wait(1000);
        cy.contains('Wijzigingen opslaan').click();

        cy.contains('Gewicht is succesvol bewerkt!').should('be.visible');
        cy.wait(1000);
    });

    it('Should edit a doelgewicht', () => {
        cy.visit('/Gewicht');
        cy.wait(1000);
        cy.get('#DoelgewichtToevoegenButton').click();
        cy.wait(1000);

        cy.get('input[placeholder="Voeg je nieuwe gewicht in."]')
            .clear()
            .type(87);
        cy.wait(1000);
        cy.contains('Wijzigingen opslaan').click();

        cy.contains('Doelgewicht is succesvol bewerkt!').should('be.visible');
        cy.wait(1000);
    });

    it('Should delete a gewicht', () => {
        cy.visit('/Gewicht');
        cy.wait(1000);
        cy.get('#EditPopUp').click();
        cy.wait(1000);
        
        cy.get('#delete-button').click();
        cy.wait(1000);

        cy.contains('Gewicht is succesvol verwijderd!').should('be.visible');
        cy.wait(1000);
    });
    

    it('Should delete a doelgewicht', () => {
        cy.visit('/Gewicht');
        cy.wait(1000);
        cy.get('#DoelgewichtToevoegenButton').click();
        cy.wait(1000);

        cy.get('#delete-button').click();
        cy.wait(1000);

        cy.contains('Doelgewicht is succesvol verwijderd!').should('be.visible');
        cy.wait(1000);
    });
});
