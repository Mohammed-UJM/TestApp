<template>
  <div class="person-list">
    <h1>Liste des personnes</h1>

    <!-- Champ de recherche par entreprise -->
    <div class="search-container">
      <label for="entrepriseSearch">Recherche par entreprise : </label>
      <input type="text" v-model="entrepriseSearch" id="entrepriseSearch">
    </div>

    <!-- Affichage des personnes -->
    <p v-if="filteredPersons.length === 0">Aucune Personne !</p>
    <table v-else>
      <thead>
        <tr>
          <th>Nom</th>
          <th>Prénom</th>
          <th>Date de Naissance</th>
          <th>Âge</th>
          <th>Emplois Actuels</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="person in filteredPersons" :key="person.id">
          <td>{{ person.nom }}</td>
          <td>{{ person.prenom }}</td>
          <td>{{ formatDate(person.dateNaissance) }}</td>
          <td>{{ calculateAge(person.dateNaissance) }}</td>
          <td>
            <ul class="emplois">
              <li v-if="person.emploisActuels.length === 0">Aucun emploi</li>
              <li v-else v-for="emploi in person.emploisActuels" :key="emploi.id">
                {{ emploi.poste }} chez {{ emploi.nomEntreprise }}
              </li>
            </ul>
          </td>
          <td>
            <button @click="viewDetails(person.id)" class="btn">Détails</button>
            <button @click="editPerson(person.id)" class="btn btn-warning">Modifier</button>
            <button @click="deletePerson(person.id)" class="btn btn-danger">Supprimer</button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
import { ref, onMounted, computed } from 'vue';
import apiClient from '@/axios';
import { useRouter } from 'vue-router';
import { formatDate } from '../utils/globalFunctions';

export default {
  name: 'PersonList',
  setup() {
    const persons = ref([]); // Référence réactive pour stocker la liste des personnes
    const router = useRouter(); // Utilisation du routeur Vue pour la navigation
    const entrepriseSearch = ref(''); // Référence réactive pour le champ de recherche par entreprise

    // Fonction pour récupérer la liste des personnes depuis l'API
    const fetchPersons = async () => {
      try {
        const response = await apiClient.get('/Personnes');
        persons.value = response.data.map(person => ({
          ...person,
          emploisActuels: person.emplois.filter(emploi => emploi.estActuel)
        })).sort((a, b) => a.nom.localeCompare(b.nom));
      } catch (error) {
        console.error('Erreur lors de la récupération des personnes:', error);
      }
    };

    // Fonction pour calculer l'âge à partir de la date de naissance
    const calculateAge = (birthDate) => {
      const birth = new Date(birthDate);
      const diff = Date.now() - birth.getTime();
      const age = new Date(diff).getUTCFullYear() - 1970;
      return age;
    };

    // Fonction pour afficher les détails d'une personne
    const viewDetails = (personId) => {
      router.push({ name: 'PersonDetails', query: { id: personId } });
    };

    // Fonction pour modifier une personne
    const editPerson = (personId) => {
      router.push({ name: 'ModifyPerson', query: { id: personId } });
    };

    // Fonction pour supprimer une personne
    const deletePerson = async (personId) => {
      try {
        await apiClient.delete(`/Personnes/${personId}`);
        fetchPersons(); // Rafraîchir la liste après suppression
      } catch (error) {
        console.error('Erreur lors de la suppression de la personne:', error);
      }
    };

    // Filtrage des personnes basé sur la recherche par entreprise
    const filteredPersons = computed(() => {
      const searchQuery = entrepriseSearch.value.trim().toLowerCase();
      if (!searchQuery) return persons.value;
      return persons.value.filter(person =>
        person.emplois.some(emploi =>
          emploi.nomEntreprise.toLowerCase() === searchQuery
        )
      );
    });

    // Appel à fetchPersons au chargement initial du composant
    onMounted(fetchPersons);

    // Exportation des variables et fonctions nécessaires au template
    return { persons, entrepriseSearch, calculateAge, viewDetails, editPerson, deletePerson, formatDate, filteredPersons };
  },
};
</script>

<style scoped>
/* Styles spécifiques au composant PersonList */
.emplois {
  margin-left: -20px;
}

.person-list {
  max-width: 1100px;
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

.search-container {
  text-align: center;
  margin-bottom: 20px;
}

.search-container label {
  font-weight: bold;
  margin-right: 10px;
}

.search-container input[type="text"] {
  padding: 5px 10px;
  font-size: 16px;
  border: 1px solid #ccc;
  border-radius: 4px;
  width: 300px;
}

table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 20px;
}

th,
td {
  padding: 10px;
  text-align: left;
  border: 1px solid #ddd;
}

th {
  background-color: #f2f2f2;
}

td {
  padding: 0px 10px;
}

.btn {
  display: inline-block;
  margin: 10px;
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
</style>
