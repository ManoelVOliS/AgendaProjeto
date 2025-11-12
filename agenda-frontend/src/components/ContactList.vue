<script setup>
import { ref, onMounted } from 'vue';
import apiService from '../services/api-service.js';

import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Button from 'primevue/button';
import Toolbar from 'primevue/toolbar'; 
import Dialog from 'primevue/dialog';

import ContactForm from './ContactForm.vue'; 

const contacts = ref([]);
const modalVisible = ref(false); 
const contactToEdit = ref(null); 

async function loadContacts() {
  try {
    const response = await apiService.getContacts(); 
    contacts.value = response.data; 
  } catch (error) {
    console.error("Error loading contacts:", error);
  }
}

function openNewModal() {
  contactToEdit.value = null; 
  modalVisible.value = true; 
}

function openEditModal(contact) { 
  contactToEdit.value = contact; 
  modalVisible.value = true; 
}

function closeModal() {
  modalVisible.value = false;
}

async function saveContact(contact) {
  try {
    if (contact.id) {
      const updateDto = {
        name: contact.name,
        email: contact.email,
        phone: contact.phone
      };
      await apiService.updateContact(contact.id, updateDto);
    } else {
      const createDto = {
        name: contact.name,
        email: contact.email,
        phone: contact.phone
      };
      await apiService.createContact(createDto);
    }

    closeModal(); 
    loadContacts(); 

  } catch (error) {
     console.error("Error saving contact:", error);

     if (error.response && error.response.status === 400) {
      if (error.response.data.errors) { 
        let messages = Object.values(error.response.data.errors).join('\n');
        alert(`Validation Errors:\n${messages}`);
      } else if (typeof error.response.data === 'string') {
        alert(error.response.data);
      } else {
        alert('An error occurred while saving.');
      }
    } else {
      alert('An unexpected error occurred.');
    }
  }
}

async function deleteContact(contact) {
  if (confirm(`Are you sure you want to delete "${contact.name}"?`)) {
    try {
      await apiService.deleteContact(contact.id);
      loadContacts(); 
    } catch (error) {
      console.error("Error deleting contact:", error);
      alert('Could not delete contact. An error occurred.');
    }
  }
}

onMounted(() => {
  loadContacts();
});

</script>

<template>
  <div class="card">
    <Toolbar class="mb-4">
      <template #end> <Button label="Add" icon="pi pi-plus" class="p-button-success" @click="openNewModal" />
      </template>
    </Toolbar>

<DataTable :value="contacts" tableStyle="min-width: 50rem">
  <Column field="name" header="Name"></Column>
  <Column field="email" header="Email"></Column>

  <Column 
    field="phone" 
    header="Phone" 
    headerStyle="text-align: center" 
    bodyClass="text-center"
  ></Column>
  
  <Column 
    header="Actions" 
    headerStyle="text-align: center"
  >
    <template #body="slotProps">
      <div class="actions-center"> 
        <Button icon="pi pi-pencil" class="p-button-rounded p-button-success" 
                @click="openEditModal(slotProps.data)" />
        
        <Button icon="pi pi-trash" class="p-button-rounded p-button-danger" 
                @click="deleteContact(slotProps.data)" />
      </div>
    </template>
  </Column>

</DataTable>

  <ContactForm 
  :visible="modalVisible" 
  :contact="contactToEdit" 
  @close="closeModal" 
  @save="saveContact"
  />
   </div>
</template>
