// src/utils/globalFunctions.js

// Fonction pour formater une date au format jour-mois-année
export function formatDate(value) {
  if (value) {
    const date = new Date(value); // Création d'un objet Date à partir de la valeur fournie
    const day = date.getDate().toString().padStart(2, "0"); // Jour du mois (avec padding)
    const month = (date.getMonth() + 1).toString().padStart(2, "0"); // Mois (ajusté pour correspondre à l'index 1-12)
    const year = date.getFullYear(); // Année au format AAAA
    return `${day}-${month}-${year}`; // Formatage de la date en jour-mois-année
  }
  return ""; // Retourne une chaîne vide si la valeur n'est pas définie
}
