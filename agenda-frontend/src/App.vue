<script setup>
import ContatoList from "./components/ContatoList.vue";
</script>

<template>
  <div id="app-container" class="p-4">
    <header>
      <h1>Minha Agenda de Contatos</h1>
    </header>

    <main>
      <ContatoList />
    </main>
  </div>
</template>

<style scoped>
/* 'scoped' significa que este CSS só se aplica a este componente */
#app-container {
  max-width: 1000px;
  margin: 20px auto;
  font-family: Arial, sans-serif;
}

header {
  margin-bottom: 20px;
  border-bottom: 2px solid #eee;
  padding-bottom: 10px;
}

/* Remove o CSS de exemplo do Vite */
.logo {
  display: none;
}
</style>