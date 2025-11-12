<script setup>
import { ref, watch } from 'vue';

import Button from 'primevue/button';
import InputText from 'primevue/inputtext';
import Dialog from 'primevue/dialog';

const props = defineProps({
  visible: Boolean, 
  contact: Object   
});

const emit = defineEmits(['close', 'save']);

const contactLocal = ref({});

watch(() => props.contact, (newValue) => {
  contactLocal.value = newValue ? { ...newValue } : { name: '', email: '', phone: '' };
});

function save() {
  emit('save', contactLocal.value);
}

function exitModal() {
  emit('close');
}
</script>

<template>
  <Dialog 
    :visible="props.visible" 
    modal 
    header="Contact Details" 
    :style="{ width: '30rem' }" 
    @update:visible="exitModal"
  >
    <div class="p-fluid">
      <div class="p-field">
        <label for="name">Name</label>
        <InputText id="name" v-model="contactLocal.name" />
      </div>
      <div class="p-field">
        <label for="email">Email</label>
        <InputText id="email" v-model="contactLocal.email" />
      </div>
      <div class="p-field">
        <label for="phone">Phone</label>
        <InputText id="phone" v-model="contactLocal.phone" p-keyfilter="int" />
      </div>
    </div>
    
    <template #footer>
      <Button label="Cancel" icon="pi pi-times" @click="exitModal" class="p-button-text" />
      <Button label="Save" icon="pi pi-check" @click="save" />
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