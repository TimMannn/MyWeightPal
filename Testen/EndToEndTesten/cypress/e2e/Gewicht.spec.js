describe('Quick login', () => {
    beforeEach(() => {
        cy.fixture('user').then((user) => {
            cy.login(user.username, user.password, user.rememberme);
        });
    });

    // ALT FLOW /////////////////////////////////////////////////////////////
    it('Should not allow adding gewicht with empty input', () => {
        cy.visit('/Gewicht');
        cy.get('#GewichtToevoegenButton').click();
        cy.wait(1000);

        cy.contains('Opslaan').click();
        cy.wait(1000);

        cy.contains('Gewicht toevoegen mislukt!').should('be.visible');

        cy.contains('Annuleer').click();
        cy.wait(1000);
    });

    it('Should not allow negative gewicht', () => {
        const testGewicht = -10;
        cy.visit('/Gewicht');
        cy.wait(1000);
        cy.get('#GewichtToevoegenButton').click();
        cy.wait(1000);

        cy.get('input[placeholder="Voer je gewicht in."]').type(testGewicht);
        cy.wait(1000);
        cy.contains('Opslaan').click();

        cy.contains('Gewicht toevoegen mislukt!').should('be.visible');
        cy.wait(1000);
    });
    ////////////////////////////////////////////////////////////////////////
    

    it('Should add a new gewicht', () => {
        const testGewicht = 80;
        cy.visit('/Gewicht');
        cy.wait(1000);
        cy.get('#GewichtToevoegenButton').click();
        cy.wait(1000);

        cy.get('input[placeholder="Voer je gewicht in."]').type(testGewicht);
        cy.wait(1000);
        cy.contains('Opslaan').click();

        cy.contains('Gewicht toegevoegd!').should('be.visible');
        cy.contains(`${testGewicht} kg`).should('be.visible');
        cy.wait(1000);
    });

    // ALT FLOW /////////////////////////////////////////////////////////////
    it('Should not allow adding doelgewicht with empty input', () => {
        cy.visit('/Gewicht');
        cy.get('#DoelgewichtToevoegenButton').click();
        cy.wait(1000);

        cy.contains('Opslaan').click();
        cy.wait(1000);

        cy.contains('Doelgewicht toevoegen mislukt!').should('be.visible');
        cy.wait(1000);
    });

    it('Should not allow negative doelgewicht', () => {
        const testGewicht = -10;
        cy.visit('/Gewicht');
        cy.wait(1000);
        cy.get('#DoelgewichtToevoegenButton').click();
        cy.wait(1000);

        cy.get('input[placeholder="Voer je doelgewicht in."]').type(testGewicht);
        cy.wait(1000);
        cy.contains('Opslaan').click();

        cy.contains('Doelgewicht toevoegen mislukt!').should('be.visible');
        cy.wait(1000);
    });
    ////////////////////////////////////////////////////////////////////////

    it('Should add a new doelgewicht', () => {
        const testDoelGewicht = 85;
        cy.visit('/Gewicht');
        cy.wait(1000);
        cy.get('#DoelgewichtToevoegenButton').click();
        cy.wait(1000);

        cy.get('input[placeholder="Voer je doelgewicht in."]').type(testDoelGewicht);
        cy.wait(1000);
        cy.contains('Opslaan').click();

        cy.contains('Doelgewicht toegevoegd!').should('be.visible');
        cy.contains(`${testDoelGewicht} kg`).should('be.visible');
        cy.wait(1000);
    });

    // ALT FLOW /////////////////////////////////////////////////////////////
    it('Should cancel edit gewicht without saving', () => {
        const origineel = 80;
        const nieuwGewicht = 83;

        cy.visit('/Gewicht');
        cy.get('#GewichtToevoegenButton').click();
        cy.wait(1000);

        cy.get('input[placeholder="Voeg je nieuwe gewicht in."]')
            .clear()
            .type(nieuwGewicht);
        cy.wait(1000);

        cy.contains('Annuleer').click();
        cy.wait(1000);

        cy.contains(`${nieuwGewicht} kg`).should('not.exist');
        cy.contains(`${origineel} kg`).should('be.visible');
    });
    ////////////////////////////////////////////////////////////////////////

    it('Should edit a gewicht', () => {
        const nieuwGewicht = 82;
        cy.visit('/Gewicht');
        cy.wait(1000);
        cy.get('#GewichtToevoegenButton').click();
        cy.wait(1000);

        cy.get('input[placeholder="Voeg je nieuwe gewicht in."]')
            .clear()
            .type(nieuwGewicht);
        cy.wait(1000);
        cy.contains('Wijzigingen opslaan').click();

        cy.contains('Gewicht is succesvol bewerkt!').should('be.visible');
        cy.contains(`${nieuwGewicht} kg`).should('be.visible');
        cy.wait(1000);
    });

    // ALT FLOW /////////////////////////////////////////////////////////////
    it('Should cancel edit doelgewicht without saving', () => {
        const origineel = 85;
        const nieuwGewicht = 88;

        cy.visit('/Gewicht');
        cy.get('#GewichtToevoegenButton').click();
        cy.wait(1000);

        cy.get('input[placeholder="Voeg je nieuwe gewicht in."]')
            .clear()
            .type(nieuwGewicht);
        cy.wait(1000);

        cy.contains('Annuleer').click();
        cy.wait(1000);

        cy.contains(`${nieuwGewicht} kg`).should('not.exist');
        cy.contains(`${origineel} kg`).should('be.visible');
    });
    ////////////////////////////////////////////////////////////////////////

    it('Should edit a doelgewicht', () => {
        const nieuwDoelgewicht = 87;
        cy.visit('/Gewicht');
        cy.wait(1000);
        cy.get('#DoelgewichtToevoegenButton').click();
        cy.wait(1000);

        cy.get('input[placeholder="Voeg je nieuwe gewicht in."]')
            .clear()
            .type(nieuwDoelgewicht);
        cy.wait(1000);
        cy.contains('Wijzigingen opslaan').click();

        cy.contains('Doelgewicht is succesvol bewerkt!').should('be.visible');
        cy.contains(`${nieuwDoelgewicht} kg`).should('be.visible');
        cy.wait(1000);
    });

    it('Should delete a gewicht', () => {
        const gewichtToDelete = 82;
        cy.visit('/Gewicht');
        cy.wait(1000);
        cy.contains(`${gewichtToDelete} kg`).should('be.visible');
        cy.get('#EditPopUp').click();
        cy.wait(1000);

        cy.get('#delete-button').click();
        cy.wait(1000);

        cy.contains('Gewicht is succesvol verwijderd!').should('be.visible');
        cy.contains(`${gewichtToDelete} kg`).should('not.exist');
        cy.wait(1000);
    });

    it('Should delete a doelgewicht', () => {
        const doelgewichtToDelete = 85;
        cy.visit('/Gewicht');
        cy.wait(1000);
        cy.get('#DoelgewichtToevoegenButton').click();
        cy.wait(1000);

        cy.get('#delete-button').click();
        cy.wait(1000);

        cy.contains('Doelgewicht is succesvol verwijderd!').should('be.visible');
        cy.contains(`${doelgewichtToDelete} kg`).should('not.exist');
        cy.wait(1000);
    });
});
