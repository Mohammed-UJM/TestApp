import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";

// Création de l'application Vue
const app = createApp(App);

// Utilisation du routeur pour gérer les routes de l'application
app.use(router);

// Montage de l'application sur l'élément DOM avec l'ID "app"
app.mount("#app");
