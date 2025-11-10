<script setup>
import { ref, watch } from 'vue';

import Button from 'primevue/button';
import InputText from 'primevue/inputtext';
import Dialog from 'primevue/dialog';

const props = defineProps({
  visible: Boolean, 
  contato: Object   
});

const emit = defineEmits(['fechar', 'salvar']);

const contatoLocal = ref({});

watch(() => props.contato, (novoValor) => {
  contatoLocal.value = novoValor ? { ...novoValor } : { nome: '', email: '', telefone: '' };
});

function salvar() {
  emit('salvar', contatoLocal.value);
}

function fecharModal() {
  emit('fechar');
}
</script>

<template>
  <Dialog 
    :visible="props.visible" 
    modal 
    header="Detalhes do Contato" 
    :style="{ width: '30rem' }" 
    @update:visible="fecharModal"
  >
    <div class="p-fluid">
      <div class="p-field">
        <label for="nome">Nome</label>
        <InputText id="nome" v-model="contatoLocal.nome" />
      </div>
      <div class="p-field">
        <label for="email">Email</label>
        <InputText id="email" v-model="contatoLocal.email" />
      </div>
      <div class="p-field">
        <label for="telefone">Telefone</label>
        <InputText id="telefone" v-model="contatoLocal.telefone" />
      </div>
    </div>
    
    <template #footer>
      <Button label="Cancelar" icon="pi pi-times" @click="fecharModal" class="p-button-text" />
      <Button label="Salvar" icon="pi pi-check" @click="salvar" />
    </template>
  </Dialog>
</template>

<style scoped>
.p-field {
  margin-bottom: 1rem;
}
.p-field label {
  display: block;
  margin-bottom: 0.5rem;
  font-weight: bold;
}
</style>