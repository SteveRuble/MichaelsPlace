import {inject} from 'aurelia-framework';
import {HttpClient} from 'aurelia-fetch-client';

@inject(HttpClient)
export class TagSteps {
  heading = 'Describe Yourself';
  tags = [];

  constructor(http) {
    this.http = http;
  }

  activate() {
    return this.http.fetch('tags/demographic')
      .then(response => response.json())
      .then(tags => this.tags = tags);
  }
}
