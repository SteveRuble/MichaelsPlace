import 'bootstrap';
import {HttpClient} from 'aurelia-fetch-client';
import {User} from 'models/user';

export function configure(aurelia) {
    aurelia.use
        .standardConfiguration()
        .developmentLogging()
        .plugin('aurelia-validation');

    //Uncomment the line below to enable animation.
    aurelia.use.plugin('aurelia-animator-css');
    //if the css animator is enabled, add swap-order="after" to all router-view elements

    let httpClient = configureHttpClient(aurelia.container);
    let user = configureUser(aurelia.container, httpClient);

    user.update()
        .then(() => aurelia.start())
        .then(() => aurelia.setRoot());
}

function configureHttpClient(container) {
    let httpClient = new HttpClient();
    httpClient.configure(config => {
        console.dir(config);
        config
            .withBaseUrl('http://localhost:8080/api/')
            .withDefaults({
                credentials: 'include',
                headers: {
                    'Accept': 'application/json',
                    'X-Requested-With': 'Fetch'
                }
            })
            .withInterceptor({
                request(request) {
                    console.debug(`-> ${request.method} ${request.url}`);
                    return request; // you can return a modified Request, or you can short-circuit the request by returning a Response
                },
                response(response) {
                    console.debug(`<- ${response.status} : ${response.url}`);
                    return response; // you can return a modified Response
                }
            });
    });

    container.registerInstance(HttpClient, httpClient);
    return httpClient;
}

function configureUser(container, httpClient) {
    var user = new User(httpClient);
    container.registerInstance(User, user);
    return user;
}