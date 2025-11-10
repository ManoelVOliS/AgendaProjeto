<script setup>
import { ref, onMounted } from 'vue';
import apiService from '../services/api-service.js';

import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Button from 'primevue/button';
import Toolbar from 'primevue/toolbar'; 
import Dialog from 'primevue/dialog';

import ContatoForm from './ContatoForm.vue'; 

const contatos = ref([]);
const modalVisivel = ref(false); 
const contatoParaEditar = ref(null); 

async function carregarContatos() {
  try {
    const response = await apiService.getContatos(); 
    contatos.value = response.data; 
  } catch (error) {
    console.error("Erro ao carregar contatos:", error);
  }
}

function abrirModalNovo() {
  contatoParaEditar.value = null; // Garante que o formulário está vazio
  modalVisivel.value = true; // Mostra o modal
}

function abrirModalEditar(contato) {
  contatoParaEditar.value = contato; // Passa o contato selecionado
  modalVisivel.value = true; // Mostra o modal
}

function fecharModal() {
  modalVisivel.value = false;
}

async function salvarContato(contato) {
  try {
    if (contato.id) {

      const updateDto = {
        nome: contato.nome,
        email: contato.email,
        telefone: contato.telefone
      };
      await apiService.updateContato(contato.id, updateDto);
    } else {

      const createDto = {
        nome: contato.nome,
        email: contato.email,
        telefone: contato.telefone
      };
      await apiService.createContato(createDto);
    }
    
    fecharModal(); // Fecha o popup
    carregarContatos(); // Recarrega a tabela
    
  } catch (error) {
    console.error("Erro ao salvar contato:", error);
    if (error.response && error.response.status === 400) {
      alert("Erro de validação: " + JSON.stringify(error.response.data.errors));
    }
  }
}

async function apagarContato(contato) {

  if (confirm(`Tem a certeza que quer apagar "${contato.nome}"?`)) {
    try {
      await apiService.deleteContato(contato.id);
      carregarContatos(); // Recarrega a tabela
    } catch (error) {
      console.error("Erro ao apagar contato:", error);
    }
  }
}

onMounted(() => {
  carregarContatos();
});

</script>

<template>
  <div class="card">
    <Toolbar class="mb-4">
      <template #start>
        <Button label="Adicionar" icon="pi pi-plus" class="p-button-success" @click="abrirModalNovo" />
      </template>
    </Toolbar>

    <DataTable :value="contatos" tableStyle="min-width: 50rem">
      <Column field="id" header="ID"></Column>
      <Column field="nome" header="Nome"></Column>
      <Column field="email" header="Email"></Column>
      <Column field="telefone" header="Telefone"></Column>
      
      <Column header="Ações">
        <template #body="slotProps">
          <Button icon="pi pi-pencil" class="p-button-rounded p-button-success p-mr-2" 
                  @click="abrirModalEditar(slotProps.data)" />
          
          <Button icon="pi pi-trash" class="p-button-rounded p-button-danger" 
                  @click="apagarContato(slotProps.data)" />
        </template>
      </Column>
    </DataTable>

    <ContatoForm 
      :visible="modalVisivel" 
      :contato="contatoParaEditar" 
      @fechar="fecharModal" 
      @salvar="salvarContato"
    />
  </div>
</template>

<style scoped>
.p-mr-2 {
  margin-right: 0.5rem;
}
.mb-4 {
  margin-bottom: 1.5rem;
}
</style>