<template>
  <div class="add-person">
    <!-- Titre de la page -->
    <h1>Ajouter une personne</h1>

    <!-- Formulaire d'ajout de personne -->
    <form @submit.prevent="addPerson" class="form">
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

      <!-- Bouton Ajouter -->
      <button type="submit" class="btn">Ajouter</button>
    </form>

    <!-- Message d'erreur global -->
    <p v-if="errorMessage" class="error">{{ errorMessage }}</p>
  </div>
</template>


<script>
import apiClient from '@/axios'; // Import du client API Axios

export default {
  data() {
    return {
      person: {
        nom: '',
        prenom: '',
        dateNaissance: ''
      },
      ageError: '', // Variable pour stocker les erreurs liées à l'âge
      errorMessage: '' // Variable pour stocker les messages d'erreur généraux
    };
  },
  methods: {
    // Méthode pour ajouter une personne
    addPerson() {
      // const age = this.calculateAge(this.person.dateNaissance); // Calcul de l'âge à partir de la date de naissance
      // if (age > 150) {
      //   this.ageError = 'Les personnes de plus de 150 ans ne peuvent pas être enregistrées.'; // Vérification de l'âge maximum
      //   return;
      // }

      // Appel API pour ajouter la personne
      apiClient.post('/personnes', this.person)
        .then(() => {
          this.$router.push('/'); // Redirection après succès
        })
        .catch(error => {
          console.error('Erreur lors de l\'ajout de la personne:', error); // Affichage de l'erreur dans la console
          this.errorMessage = 'Tous les champs sont obligatoire, et l\'age ne doit pas dépasser 150 ans.'; // Message d'erreur affiché à l'utilisateur
        });
    },
    // Méthode pour calculer l'âge à partir de la date de naissance
    // calculateAge(dateNaissance) {
    //   const birthDate = new Date(dateNaissance);
    //   const ageDiff = Date.now() - birthDate.getTime();
    //   const ageDate = new Date(ageDiff);
    //   return Math.abs(ageDate.getUTCFullYear() - 1970); // Renvoie l'âge en années
    // },
    // Méthode pour valider l'âge lors de la saisie de la date de naissance
    // validateAge() {
    //   const age = this.calculateAge(this.person.dateNaissance); // Calcul de l'âge actuel
    //   if (age > 150) {
    //     this.ageError = 'Les personnes de plus de 150 ans ne peuvent pas être enregistrées.'; // Affichage de l'erreur si l'âge est invalide
    //   } else {
    //     this.ageError = ''; // Réinitialisation de l'erreur si l'âge est valide
    //   }
    // }
  }
};
</script>


<style scoped>
/* Style pour la page */
.add-person {
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
