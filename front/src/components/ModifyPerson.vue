<template>
  <!-- Page de modification de la personne -->
  <div class="modify-person">
    <h1>Modifier la personne</h1>

    <!-- Formulaire de modification -->
    <form @submit.prevent="updatePerson" class="form">
      <!-- Champ Nom -->
      <div class="form-group">
        <label for="nom">Nom :</label>
        <input type="text" v-model="person.nom" id="nom">
      </div>

      <!-- Champ Prénom -->
      <div class="form-group">
        <label for="prenom">Prénom :</label>
        <input type="text" v-model="person.prenom" id="prenom">
      </div>

      <!-- Champ Date de naissance avec validation d'âge -->
      <div class="form-group">
        <label for="dateNaissance">Date de naissance :</label>
        <input type="date" v-model="person.dateNaissance" @blur="validateAge" id="dateNaissance">
        <p v-if="ageError" class="error">{{ ageError }}</p> <!-- Message d'erreur si l'âge est invalide -->
      </div>

      <!-- Bouton Modifier -->
      <button type="submit" class="btn">Modifier</button>
    </form>

    <!-- Message d'erreur global -->
    <p v-if="errorMessage" class="error">{{ errorMessage }}</p>
  </div>
</template>

<script>
import { ref, onMounted } from 'vue';
import apiClient from '@/axios'; // Import du client API Axios
import { useRoute, useRouter } from 'vue-router'; // Import des hooks de Vue Router

export default {
  name: 'ModifyPerson',
  setup() {
    const route = useRoute(); // Obtenir les données de la route
    const router = useRouter(); // Gestionnaire de routeur
    const person = ref({ // Référence réactive pour les données de la personne
      nom: '',
      prenom: '',
      dateNaissance: ''
    });
    const ageError = ref('');
    const errorMessage = ref('');

    // Fonction pour récupérer les détails de la personne à modifier
    const fetchPerson = async (id) => {
      try {
        const response = await apiClient.get(`/Personnes/${id}`);
        const fetchedPerson = response.data;
        fetchedPerson.dateNaissance = fetchedPerson.dateNaissance.split('T')[0]; // Conversion du format de date
        person.value = fetchedPerson; // Mise à jour des données de la personne
      } catch (error) {
        console.error('Erreur lors de la récupération de la personne:', error);
        errorMessage.value = 'Erreur lors de la récupération de la personne. Veuillez réessayer !';
      }
    };

    // Fonction pour mettre à jour les détails de la personne
    const updatePerson = async () => {
      try {
        await apiClient.put(`/Personnes/${route.query.id}`, person.value); // Appel API pour mettre à jour la personne
        router.push('/'); // Redirection après la mise à jour réussie
      } catch (error) {
        console.error('Erreur lors de la modification de la personne:', error);
        errorMessage.value = 'Tous les champs sont obligatoire, et l\'age ne doit pas dépasser 150 ans.';
      }
    };

    // Fonction pour calculer l'âge à partir de la date de naissance
    // const calculateAge = (dateNaissance) => {
    //   const birthDate = new Date(dateNaissance);
    //   const ageDiff = Date.now() - birthDate.getTime();
    //   const ageDate = new Date(ageDiff);
    //   return Math.abs(ageDate.getUTCFullYear() - 1970); // Calcul de l'âge en années
    // };

    // Fonction pour valider l'âge lors de la saisie de la date de naissance
    // const validateAge = () => {
    //   const age = calculateAge(person.value.dateNaissance); // Calcul de l'âge actuel
    //   if (age > 150) {
    //     ageError.value = 'Les personnes de plus de 150 ans ne peuvent pas être enregistrées.';
    //   } else {
    //     ageError.value = ''; // Réinitialisation de l'erreur si l'âge est valide
    //   }
    // };

    // Récupération des données de la personne au chargement du composant
    onMounted(() => {
      fetchPerson(route.query.id);
    });

    // Retour des données et les méthodes utilisées dans le composant
    return { person, ageError, errorMessage, updatePerson };
  },
};
</script>

<style scoped>
/* Style du composant */
.modify-person {
  max-width: 600px;
  margin: 30px auto;
  padding: 20px;
  border: 1px solid #ddd;
  border-radius: 8px;
  background-color: #f9f9f9;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

h1 {
  text-align: center;
  margin-bottom: 20px;
}

.form {
  display: flex;
  flex-direction: column;
  gap: 15px;
}

.form-group {
  display: flex;
  flex-direction: column;
}

label {
  margin-bottom: 5px;
  font-weight: bold;
}

input[type="text"],
input[type="date"] {
  padding: 10px;
  font-size: 16px;
  border: 1px solid #ccc;
  border-radius: 4px;
  box-sizing: border-box;
}

input[type="text"]:focus,
input[type="date"]:focus {
  border-color: #007bff;
  outline: none;
}

.btn {
  padding: 10px 20px;
  font-size: 16px;
  color: #fff;
  background-color: #007bff;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

.btn:hover {
  background-color: #0056b3;
}

.error {
  color: red;
  font-size: 14px;
  margin-top: 5px;
}
</style>
