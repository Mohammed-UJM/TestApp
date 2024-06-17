// src/axios.js
import axios from "axios";

// Création d'une instance d'axios avec une configuration personnalisée
const apiClient = axios.create({
  baseURL: "http://localhost:63111/api", // l'URL de notre API ASP.NET
  headers: {
    "Content-Type": "application/json", // Type de contenu des requêtes JSON
  },
});

// Exportation de l'instance d'axios pour utilisation dans d'autres fichiers
export default apiClient;
