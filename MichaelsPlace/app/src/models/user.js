import {HttpClient} from 'aurelia-fetch-client';

export class User {
    constructor(httpClient) {
        this.httpClient = httpClient;
        this.anonymous = true;
        this.roles = [];
    }

    update() {
        return this.httpClient.fetch('user/claims')
            .then(r => r.json())
            .then(claims => {
                this.anonymous = claims.length === 0;
                for (var claim of claims) {
                    switch (claim.type) {
                        case 'username':
                            this.username = claim.value;
                            break;
                        case 'id':
                            this.id = claim.value;
                            break;
                        case 'role':
                            this.roles.push(claim.value);
                            break;
                    }
                }
            });
    }
}