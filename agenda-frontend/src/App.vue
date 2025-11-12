<script setup>
import ContactList from "./components/ContactList.vue";
</script>

<template>
  <div id="app-container" class="p-4">
    <header>
      <h1>Contact list</h1>
    </header>

    <main>
      <ContactList />
    </main>
  </div>
</template>

<style>
  body {
    font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, Helvetica, Arial, sans-serif, "Apple Color Emoji", "Segoe UI Emoji", "Segoe UI Symbol";
    margin: 0;
    padding: 0;
    background-color: #f4f4f4; 
  }

  .text-center {
    text-align: center;
  }

  .actions-center {
    display: flex;
    justify-content: center;
    gap: 0.5rem;
  }

  .card {
    background: #ffffff;
    border-radius: 8px;
    box-shadow: 0 4px 12px rgba(0,0,0,0.05);
    padding: 2rem; 
  }

  .text-center {
    text-align: center;
  }

  .actions-center {
    display: flex;
    justify-content: center;
    gap: 0.5rem; 
  }

  .p-field {
    margin-bottom: 1.25rem;
  }
  .p-field label {
    display: block;
    margin-bottom: 0.5rem;
    font-weight: 600; 
  }
</style>

<style scoped>
  #app-container {
    max-width: 1000px;
    margin: 40px auto; 
  }

  header {
    margin-bottom: 20px;
    border-bottom: 2px solid #eee;
    padding-bottom: 10px;
    text-align: center; 
  }

  h1 {
    font-size: 2.5rem;
    font-weight: 700;
    color: #333;
    margin: 0;
  }

  .logo {
    display: none;
  }
</style>