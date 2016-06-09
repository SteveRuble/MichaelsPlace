import 'bootstrap';
import { HttpClient } from 'aurelia-fetch-client';

export function configure(aurelia) {
    aurelia.use
        .standardConfiguration()
        .developmentLogging();

    //Uncomment the line below to enable animation.
    aurelia.use.plugin('aurelia-animator-css');
    //if the css animator is enabled, add swap-order="after" to all router-view elements

    //Anyone wanting to use HTMLImports to load views, will need to install the following plugin.
    //aurelia.use.plugin('aurelia-html-import-template-loader')

    configureHttpClient(aurelia.container);
    aurelia.start().then(() => aurelia.setRoot());
}

function configureHttpClient(container){
    let httpClient = new HttpClient();
    httpClient.configure(config => {
        config
            .withBaseUrl('http://localhost:8080/api/')
            .withDefaults({
                credentials: 'same-origin',
                headers: {
                    'Accept': 'application/json',
                    'X-Requested-With': 'Fetch'
                }
            })
            .withInterceptor({
                request(request) {
                    console.debug(`→ ${request.method} ${request.url}`);
                    return request; // you can return a modified Request, or you can short-circuit the request by returning a Response
                },
                response(response) {
                    console.debug(`← ${response.status} : ${response.url}`);
                    return response; // you can return a modified Response
                }
            });
    });

    container.registerInstance(HttpClient, httpClient);
}