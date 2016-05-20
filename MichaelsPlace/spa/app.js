export class App {
     
    constructor() {
        this.message = "";
    }
 
    activate() {
        this.message = "Hello, World!";
    }
 
    changeMessage() {
        this.message = "Goodbye!";
    }
 
}