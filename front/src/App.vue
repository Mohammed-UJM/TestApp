<!-- src/App.vue -->
<template>
  <div id="app" class="app-container">
    <h1>Bienvenue dans l'App de gestion des Personnes</h1>
    <nav>
      <router-link to="/" class="nav-link">Liste des personnes</router-link> |
      <router-link to="/add-person" class="nav-link">Ajouter une personne</router-link>
    </nav>
    <router-view /> <!-- Composant pour afficher les vues des routes -->
  </div>
</template>

<script>
export default {
  name: 'App', // Nom du composant Vue

  // Composant principal de l'application
}
</script>

<style>
/* Styles CSS pour l'application */
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}

.app-container {
  max-width: 1200px;
  margin: 0 auto;
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

nav {
  text-align: center;
  margin-bottom: 20px;
}

.nav-link {
  display: inline-block;
  padding: 10px 20px;
  font-size: 16px;
  color: #007bff;
  text-decoration: none;
  border: 1px solid #007bff;
  border-radius: 4px;
  background-color: #fff;
  transition: background-color 0.3s, color 0.3s;
}

.nav-link:hover {
  background-color: #007bff;
  color: #fff;
}
</style>
