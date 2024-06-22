<template>
  <div v-if="person" class="person-details">
    <h1>{{ person.prenom }} {{ person.nom }}</h1>
    <p>Née le : {{ formatDate(person.dateNaissance) }} (Âge : {{ calculateAge(person.dateNaissance) }})</p>

    <button @click="showAddEmploiForm = !showAddEmploiForm" class="btn">
      {{ showAddEmploiForm ? 'Annuler' : 'Ajouter un emploi' }}
    </button>

    <!-- Bouton avec formulaire de ajout emploi -->
    <div v-if="showAddEmploiForm" class="add-emploi-form">
      <h3>Ajouter un emploi</h3>
      <form @submit.prevent="addEmploi">
        <div class="form-group">
          <label for="nomEntreprise">Nom de l'entreprise :</label>
          <input type="text" v-model="newEmploi.nomEntreprise" id="nomEntreprise" required>
        </div>
        <div class="form-group">
          <label for="poste">Poste :</label>
          <input type="text" v-model="newEmploi.poste" id="poste" required>
        </div>
        <div class="form-group">
          <label for="dateDebut">Date de début :</label>
          <input type="date" v-model="newEmploi.dateDebut" id="dateDebut" required>
        </div>
        <div class="form-group" v-if="!newEmploi.estActuel">
          <label for="dateFin">Date de fin :</label>
          <input type="date" v-model="newEmploi.dateFin" id="dateFin" :required="!newEmploi.estActuel">
        </div>
        <div class="form-group">
          <input type="checkbox" v-model="newEmploi.estActuel" id="estActuel">
          <label for="estActuel">Poste Actuel</label>
        </div>
        <button type="submit" class="btn">Ajouter</button>
      </form>
      <p v-if="errorMessage" class="error">{{ errorMessage }}</p>
    </div>

    <h2>Emplois</h2>

    <!-- Filtres par date -->
    <label>Recherche par date :</label>
    <div class="filter-container">
      <label for="dateStart">Date de début </label>
      <input type="date" v-model="dateStart" id="dateStart">
      <label for="dateEnd">Date de fin </label>
      <input type="date" v-model="dateEnd" id="dateEnd">
      <button @click="filterEmplois" class="btn">Filtrer</button>
      <button @click="resetFilter" class="btn">Reset</button>
    </div>
    <p v-if="dateErrorMessage" class="error">{{ dateErrorMessage }}</p>

    <!-- Affichage des emploi sinon aucun emploi ! -->
    <p v-if="filteredEmplois.length === 0">Aucun Emploi !</p>
    <table v-else>
      <thead>
        <tr>
          <th>Nom de l'entreprise</th>
          <th>Poste</th>
          <th>Date de début</th>
          <th>Date de fin</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="emploi in filteredEmplois" :key="emploi.id">
          <td>{{ emploi.nomEntreprise }}</td>
          <td>{{ emploi.poste }}</td>
          <td>{{ formatDate(emploi.dateDebut) }}</td>
          <td>{{ emploi.dateFin ? formatDate(emploi.dateFin) : 'Présent' }}</td>
          <td>
            <button @click="openModifyEmploiModal(emploi)" class="btn-actions btn-warning">Modifier</button>
            <button @click="deleteEmploi(emploi.id)" class="btn-actions btn-danger">Supprimer</button>
          </td>
        </tr>
      </tbody>
    </table>

    <!-- Modèle de dialogue pour la modification d'emploi avec son formulaire -->
    <div v-if="showModifyEmploiModal" class="modal">
      <div class="modal-content">
        <span @click="closeModifyEmploiModal" class="close">&times;</span>
        <h3>Modifier un emploi</h3>
        <form @submit.prevent="updateEmploi">
          <div class="form-group">
            <label for="nomEntreprise">Nom de l'entreprise :</label>
            <input type="text" v-model="selectedEmploi.nomEntreprise" id="nomEntreprise" required>
          </div>
          <div class="form-group">
            <label for="poste">Poste :</label>
            <input type="text" v-model="selectedEmploi.poste" id="poste" required>
          </div>
          <div class="form-group">
            <label for="dateDebut">Date de début :</label>
            <input type="date" v-model="selectedEmploi.dateDebut" id="dateDebut" required>
          </div>
          <div class="form-group" v-if="!selectedEmploi.estActuel">
            <label for="dateFin">Date de fin :</label>
            <input type="date" v-model="selectedEmploi.dateFin" id="dateFin" :required="!selectedEmploi.estActuel">
          </div>
          <div class="form-group">
            <input type="checkbox" v-model="selectedEmploi.estActuel" id="estActuel">
            <label for="estActuel">Poste Actuel</label>
          </div>
          <button type="submit" class="btn">Modifier</button>
        </form>
        <p v-if="errorMessage" class="error">{{ errorMessage }}</p>
      </div>
    </div>
  </div>
</template>

<script>
import apiClient from '@/axios';
import { formatDate } from '../utils/globalFunctions';

export default {
  name: 'PersonDetails',
  data() {
    return {
      person: null,
      showAddEmploiForm: false,
      newEmploi: {
        nomEntreprise: '',
        poste: '',
        dateDebut: '',
        dateFin: '',
        estActuel: false,
        personneId: null
      },
      selectedEmploi: {
        id: null,
        nomEntreprise: '',
        poste: '',
        dateDebut: '',
        dateFin: '',
        estActuel: false,
        personneId: null
      },
      dateStart: '',
      dateEnd: '',
      filteredEmplois: [],
      errorMessage: '',
      dateErrorMessage: '',
      showModifyEmploiModal: false
    };
  },
  methods: {
    // recuperation de la personne
    async fetchPerson(id) {
      try {
        const response = await apiClient.get(`/Personnes/${id}`);
        this.person = response.data;
        this.newEmploi.personneId = id;
        this.filteredEmplois = this.person.emplois; // Initialiser filteredEmplois avec tous les emplois
      } catch (error) {
        console.error("Erreur lors de la récupération de la personne", error);
      }
    },
    // Calcul age à partir de la date de naissance
    calculateAge(birthDate) {
      const birth = new Date(birthDate);
      const diff = Date.now() - birth.getTime();
      const age = new Date(diff).getUTCFullYear() - 1970;
      return age;
    },
    formatDate,
    async addEmploi() {
      this.errorMessage = '';

      // Vérification des dates
      if (!this.newEmploi.estActuel && !this.validateDates(this.newEmploi.dateDebut, this.newEmploi.dateFin)) {
        this.errorMessage = 'La date de fin ne peut pas être antérieure à la date de début.';
        return;
      }

      try {
        // Si estActuel est coché, définissez dateFin à null
        if (this.newEmploi.estActuel) {
          this.newEmploi.dateFin = null;
        }

        const response = await apiClient.post('/Emplois', this.newEmploi);
        this.person.emplois.push(response.data);
        this.resetEmploiForm();
      } catch (error) {
        console.error("Erreur lors de l'ajout de l'emploi", error);
        this.errorMessage = "Erreur lors de l'ajout de l'emploi.";
      }
    },
    async deleteEmploi(emploiId) {
      console.log("test contenu personne");
      console.log(emploiId);
      console.log("test contenu personne");
      try {
        await apiClient.delete(`/Emplois/${emploiId}`);
        // Mettre à jour la liste des emplois affichés après suppression
        this.person.emplois = this.person.emplois.filter(e => e.id !== emploiId);
        this.resetFilter();
      } catch (error) {
        console.error("Erreur lors de la suppression de l'emploi", error);
      }
    },
    openModifyEmploiModal(emploi) {
      // Pré-remplir le formulaire avec les détails de l'emploi sélectionné
      const selectedEmploi = { ...emploi }; // Copie de l'objet emploi
      // Formater la date de début
      if (selectedEmploi.dateDebut) {
        selectedEmploi.dateDebut = selectedEmploi.dateDebut.split('T')[0];
      }
      // Formater la date de fin si elle existe
      if (selectedEmploi.dateFin) {
        selectedEmploi.dateFin = selectedEmploi.dateFin.split('T')[0];
      }
      this.selectedEmploi = selectedEmploi;
      this.showModifyEmploiModal = true;
    },

    closeModifyEmploiModal() {
      // Réinitialiser les données de l'emploi sélectionné et cacher le modèle
      this.selectedEmploi = {
        id: null,
        nomEntreprise: '',
        poste: '',
        dateDebut: '',
        dateFin: '',
        estActuel: false,
        personneId: null
      };
      this.showModifyEmploiModal = false;
    },

    async updateEmploi() {
      this.errorMessage = '';

      // Valider les dates si nécessaire
      if (!this.selectedEmploi.estActuel && !this.validateDates(this.selectedEmploi.dateDebut, this.selectedEmploi.dateFin)) {
        this.errorMessage = 'La date de fin ne peut pas être antérieure à la date de début.';
        return;
      }

      try {
        // Si estActuel est coché, définir dateFin à null
        if (this.selectedEmploi.estActuel) {
          this.selectedEmploi.dateFin = null;
        }

        // Envoyer la requête de mise à jour à l'API
        await apiClient.put(`/Emplois/${this.selectedEmploi.id}`, this.selectedEmploi);

        // Mettre à jour les détails de l'emploi dans la liste affichée
        const index = this.person.emplois.findIndex(e => e.id === this.selectedEmploi.id);
        if (index !== -1) {
          this.person.emplois.splice(index, 1, { ...this.selectedEmploi });
        }

        // Fermer le modèle après la mise à jour réussie
        this.closeModifyEmploiModal();
      } catch (error) {
        console.error("Erreur lors de la mise à jour de l'emploi", error);
        this.errorMessage = "Erreur lors de la mise à jour de l'emploi.";
      }
    },
    // Fonction de validation des dates
    validateDates(debut, fin) {
      if (fin && new Date(fin) < new Date(debut)) {
        return false;
      }
      return true;
    },
    resetEmploiForm() {
      this.newEmploi = {
        nomEntreprise: '',
        poste: '',
        dateDebut: '',
        dateFin: '',
        estActuel: false,
        personneId: this.person.id
      };
      this.showAddEmploiForm = false;
      this.errorMessage = '';
    },
    filterEmplois() {

      this.dateErrorMessage = '';

      // Vérifier si les deux dates sont fournies
      if (!this.dateStart || !this.dateEnd) {
        this.dateErrorMessage = 'Les deux dates doivent être fournies.';
        return;
      }

      // Vérifier si dateStart est inférieure ou égale à dateEnd
      if (this.dateStart && this.dateEnd && new Date(this.dateStart) > new Date(this.dateEnd)) {
        this.dateErrorMessage = 'La date de début ne peut pas être postérieure à la date de fin.';
        return;
      }

      const start = new Date(this.dateStart);
      const end = new Date(this.dateEnd);
      start.setHours(0, 0, 0, 0); // Définir heures, minutes, secondes et millisecondes à zéro
      end.setHours(0, 0, 0, 0); // meme chose

      this.filteredEmplois = this.person.emplois.filter(emploi => {
        const debut = new Date(emploi.dateDebut);
        const fin = emploi.dateFin ? new Date(emploi.dateFin) : new Date();

        debut.setHours(0, 0, 0, 0); // Définir heures, minutes, secondes et millisecondes à zéro
        fin.setHours(0, 0, 0, 0); // meme chose

        return (debut <= start && fin >= end) ||
          (debut >= start && debut <= end) ||
          (fin >= start && fin <= end);
      });
    },
    resetFilter() {
      this.dateStart = '';
      this.dateEnd = '';
      this.filteredEmplois = this.person.emplois;
    }
  },
  beforeMount() {
    const id = this.$route.query.id;
    this.fetchPerson(id);
  }
};
</script>


<style scoped>
.person-details {
  max-width: 1000px;
  margin: 30px auto;
  padding: 20px;
  border: 1px solid #ddd;
  border-radius: 8px;
  background-color: #f9f9f9;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

h1,
h2 {
  text-align: center;
  margin: 20px 0;
}

p {
  margin-bottom: 10px;
  text-align: center;
}

table {
  width: 100%;
  border-collapse: collapse;
  margin: 20px 0;
}

th,
td {
  padding: 10px;
  text-align: left;
  border: 1px solid #ddd;
}

td {
  padding: 0px 10px;
}

th {
  background-color: #f2f2f2;
}

.btn {
  display: block;
  width: 200px;
  padding: 10px;
  margin: 20px auto;
  font-size: 16px;
  color: #fff;
  background-color: #007bff;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  text-align: center;
}

.btn:hover {
  background-color: #0056b3;
}

.add-emploi-form {
  max-width: 600px;
  margin: 20px auto;
  padding: 20px;
  border: 1px solid #ddd;
  border-radius: 8px;
  background-color: #fff;
}

.form-group {
  margin-bottom: 15px;
}

label {
  display: block;
  margin-bottom: 5px;
  font-weight: bold;
}

input[type="text"],
input[type="date"],
input[type="checkbox"] {
  width: 100%;
  padding: 10px;
  font-size: 16px;
  border: 1px solid #ccc;
  border-radius: 4px;
  box-sizing: border-box;
}

input[type="text"]:focus,
input[type="date"]:focus,
input[type="checkbox"]:focus {
  border-color: #007bff;
  outline: none;
}

.error {
  color: red;
  font-size: 14px;
  margin-top: 5px;
  text-align: center;
}

.filter-container {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 10px;
  margin: 20px 0;
}

.filter-container label {
  font-weight: bold;
}

.filter-container input {
  max-width: 150px;
}

.filter-container .btn {
  max-width: 100px;
  margin: 0;
}

.btn-actions {
  display: inline-block;
  margin: 10px 10px;
  padding: 10px 20px;
  font-size: 16px;
  color: #fff;
  background-color: #007bff;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  text-align: center;
}

.btn:hover {
  background-color: #0056b3;
}

.btn-danger {
  background-color: #dc3545;
}

.btn-danger:hover {
  background-color: #c82333;
}

.btn-warning {
  background-color: #ff9800;
}

.btn-warning:hover {
  background-color: #e68900;
}

/* Ajoutez ici le style pour le modèle de dialogue/modal si nécessaire */
.modal {
  display: block;
  /* Afficher le modèle */
  position: fixed;
  z-index: 1000;
  top: 10;
  bottom: 0;
  left: 0;
  top: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  /* Fond semi-transparent */
}

.modal-content {
  background-color: #fefefe;
  margin: 10% auto;
  padding: 20px;
  border: 1px solid #888;
  width: 80%;
  max-width: 600px;
  border-radius: 8px;
  position: relative;
}

.close {
  color: #aaa;
  float: right;
  font-size: 28px;
  font-weight: bold;
}

.close:hover,
.close:focus {
  color: black;
  text-decoration: none;
  cursor: pointer;
}
</style>