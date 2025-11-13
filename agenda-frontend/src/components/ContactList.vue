<script setup>
import { ref, onMounted } from 'vue';
import apiService from '../services/api-service.js';

import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Button from 'primevue/button';
import Card from 'primevue/card'; 

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
  <Card class="contact-card">
    
    <template #header>
      <div class="card-header">
        <div class="header-left">
          <i class="pi pi-users text-icon"></i>
          <div>
            <h2 class="header-title">All Contacts</h2>
          </div>
        </div>
        <div class="header-right">
          <Button label="Add" icon="pi pi-plus" class="p-button-success" @click="openNewModal" />
        </div>
      </div>
    </template>

    <template #content>
      <DataTable 
        :value="contacts" 
        tableStyle="min-width: 50rem"
        class="contact-table"
      >
        <Column 
          field="id" 
          header="ID" 
          headerStyle="text-align: center" 
          bodyClass="text-center"
        ></Column>

        <Column field="name" header="Name" :sortable="true">
          <template #body="{ data }">
            <div class="name-cell">
              <span class="font-medium">{{ data.name }}</span>
            </div>
          </template>
        </Column>

        <Column field="email" header="Email" :sortable="true">
          <template #body="{ data }">
            <div class="email-cell">
              <i class="pi pi-envelope"></i>
              <span>{{ data.email }}</span>
            </div>
          </template>
        </Column>

        <Column field="phone" header="Phone" bodyClass="text-center" :sortable="true">
           <template #body="{ data }">
            <div class="phone-cell">
              <i class="pi pi-phone"></i>
              <span>{{ data.phone }}</span>
            </div>
          </template>
        </Column>

        <Column header="Actions" headerStyle="text-align: center">
          <template #body="{ data }">
            <div class="actions-center"> 
              <Button 
                icon="pi pi-pencil" 
                class="p-button-rounded p-button-text p-button-success"
                @click="openEditModal(data)" />
              
              <Button 
                icon="pi pi-trash" 
                class="p-button-rounded p-button-text p-button-danger" 
                @click="deleteContact(data)" />
            </div>
          </template>
        </Column>

        <template #empty>
          <div class="empty-state">
            <i class="pi pi-users"></i>
            <p>No contacts found.</p>
          </div>
        </template>

      </DataTable>
    </template>
  </Card>

  <ContactForm 
    :visible="modalVisible" 
    :contact="contactToEdit" 
    @close="closeModal" 
    @save="saveContact"
  />
</template>

<style scoped>
.mb-4 {
  margin-bottom: 0;
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1.5rem;
  border-bottom: 1px solid #e0e0e0; 
}
.header-left {
  display: flex;
  align-items: center;
  gap: 1rem;
}
.text-icon {
  font-size: 1.5rem;
  color: #10b981; 
}
.header-title {
  font-size: 1.25rem;
  font-weight: 600;
  margin: 0;
}
.header-subtitle {
  font-size: 0.875rem;
  color: #6c757d;
  margin: 0;
}

.table-toolbar {
  padding: 1.5rem;
  padding-bottom: 0;
}

.w-full {
  width: 100%;
}

.name-cell,
.email-cell,
.phone-cell {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}
.font-medium {
  font-weight: 500;
}
.email-cell i,
.phone-cell i {
  color: #6c757d;
}

.actions-center {
  display: flex;
  justify-content: center;
  gap: 0.5rem;
}

.empty-state {
  text-align: center;
  padding: 4rem;
  color: #adb5bd;
}
.empty-state i {
  font-size: 4rem;
  margin-bottom: 1rem;
}
</style>