export class Session {

    getItem(key) {
        return Window.sessionStorage.getItem(key);
    }

    setItem(key, value) {
        Window.sessionStorage.setItem(key, value);
    }

}