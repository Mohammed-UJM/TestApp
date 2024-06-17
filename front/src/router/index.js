// src/router/index.js
import { createRouter, createWebHistory } from "vue-router";
import PersonList from "@/components/PersonList.vue";
import AddPerson from "@/components/AddPerson.vue";
import ModifyPerson from "@/components/ModifyPerson.vue";
import PersonDetails from "@/components/PersonDetails.vue";

// Définition des routes de l'application
const routes = [
  {
    path: "/",
    name: "PersonList", // Nom de la route
    component: PersonList, // Composant associé à la route
  },
  {
    path: "/add-person",
    name: "AddPerson",
    component: AddPerson,
  },
  {
    path: "/modify-person",
    name: "ModifyPerson",
    component: ModifyPerson,
  },
  {
    path: "/person-details",
    name: "PersonDetails",
    component: PersonDetails,
  },
];

// Création du routeur 
const router = createRouter({
  history: createWebHistory(process.env.BASE_URL), // Utilisation de l'historique web
  routes, // Liste des routes définies ci-dessus
});

export default router;
