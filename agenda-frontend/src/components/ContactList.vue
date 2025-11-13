<script setup>
import { ref, onMounted, computed } from 'vue';
import apiService from '../services/api-service.js';

import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Button from 'primevue/button';
import Card from 'primevue/card';
import Dialog from 'primevue/dialog';
import InputText from 'primevue/inputtext';
import Avatar from 'primevue/avatar';
import IconField from 'primevue/iconfield';
import InputIcon from 'primevue/inputicon';
import { useToast } from 'primevue';

const toast = useToast();

import ContactForm from './ContactForm.vue';

const contacts = ref([]);
const modalVisible = ref(false);
const contactToEdit = ref(null);
const searchQuery = ref('');
const deleteDialogVisible = ref(false);
const contactToDelete = ref(null);

const filteredContacts = computed(() => {
  if (!searchQuery.value) {
    return contacts.value;
  }
  const query = searchQuery.value.toLowerCase();
  return contacts.value.filter(contact =>
    contact.name.toLowerCase().includes(query) ||
    contact.email.toLowerCase().includes(query) ||
    contact.phone.includes(query)
  );
});

async function loadContacts() {
  try {
    const response = await apiService.getContacts();
    contacts.value = response.data;
  } catch (error) {
    console.error("Erro ao carregar contatos:", error);
  }
}

function openNewModal() {
  contactToEdit.value = null;
  modalVisible.value = true;
}

function openEditModal(contact) {
  contactToEdit.value = { ...contact };
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
    console.log(error)
    console.error("Erro ao salvar contato:", error);
    if (error.response && error.response.status === 400) {
      if (error.response.data) {
        // let messages = (error.response.data);
        // toast.add({
        // severity: "error",
        // summary: "Erro",
        // detail: messages,
        // life: 3000,
        // });
        alert(error.response.data)
      } else if (typeof error.response.data === 'string') {
        alert(error.response.data);
      } else {
        alert('Ocorreu um erro ao salvar o contato.');
      }
    } else {
      alert('Ocorreu um erro inesperado no servidor.');
    }
  }
}

function confirmDeleteContact(contact) {
  contactToDelete.value = contact;
  deleteDialogVisible.value = true;
}

async function deleteContact() {
  if (!contactToDelete.value) return;
  try {
    await apiService.deleteContact(contactToDelete.value.id);
    loadContacts();
  } catch (error) {
    console.error("Erro ao apagar contato:", error);
    alert('Ocorreu um erro ao apagar o contato.');
  } finally {
    deleteDialogVisible.value = false;
    contactToDelete.value = null;
  }
}

const getInitials = (name) => {
  if (!name) return '';
  return name
    .split(' ')
    .map(word => word[0])
    .slice(0, 2)
    .join('')
    .toUpperCase();
};

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
            <p class="header-subtitle">{{ contacts.length }} total contacts</p>
          </div>
        </div>
        <div class="header-right">
          <Button label="Add" icon="pi pi-plus" class="p-button-success" @click="openNewModal" />
        </div>
      </div>
    </template>

    <template #content>
      <div class="table-toolbar">
        <IconField iconPosition="left">
          <InputIcon>
            <i class="pi pi-search" />
          </InputIcon>
          <InputText 
            v-model="searchQuery"
            placeholder="Search contacts..." 
            class="w-full"
          />
        </IconField>
      </div>

      <DataTable 
        :value="filteredContacts" 
        tableStyle="min-width: 50rem"
        :paginator="true" 
        :rows="5"
        :rowsPerPageOptions="[5, 10, 20]"
        class="contact-table"
      >
        <Column 
          field="name" 
          header="Name" 
          :sortable="true"
          style="width: 35%"
        >
          <template #body="{ data }">
            <div class="name-cell">
              <Avatar :label="getInitials(data.name)" shape="circle" class="name-avatar" />
              <span class="font-medium">{{ data.name }}</span>
            </div>
          </template>
        </Column>

        <Column 
          field="email" 
          header="Email" 
          :sortable="true"
          style="width: 30%"
        >
          <template #body="{ data }">
            <div class="email-cell">
              <i class="pi pi-envelope"></i>
              <span>{{ data.email }}</span>
            </div>
          </template>
        </Column>

        <Column 
          field="phone" 
          header="Phone" 
          bodyClass="text-center" 
          :sortable="true"
          style="width: 20%"
        >
           <template #body="{ data }">
            <div class="phone-cell">
              <i class="pi pi-phone"></i>
              <span>{{ data.phone }}</span>
            </div>
          </template>
        </Column>

        <Column 
          header="Actions" 
          headerStyle="text-align: center"
          style="width: 15%"
        >
          <template #body="{ data }">
            <div class="actions-center"> 
              <Button 
                icon="pi pi-pencil" 
                class="p-button-rounded p-button-text p-button-success"
                @click="openEditModal(data)" />
              
              <Button 
                icon="pi pi-trash" 
                class="p-button-rounded p-button-text p-button-danger" 
                @click="confirmDeleteContact(data)" />
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

  <Dialog 
    v-model:visible="deleteDialogVisible" 
    modal 
    header="Confirm Deletion" 
    :style="{ width: '400px' }"
  >
    <div class="delete-dialog-content">
      <i class="pi pi-exclamation-triangle delete-icon"></i>
      <div>
        <p>Tem a certeza que quer apagar este contato?</p>
        <p class="delete-subtitle">Esta ação não pode ser desfeita.</p>
      </div>
    </div>
    <template #footer>
      <Button label="Cancelar" text severity="secondary" @click="deleteDialogVisible = false" />
      <Button 
        label="Apagar" 
        icon="pi pi-trash" 
        severity="danger"
        @click="deleteContact" 
      />
    </template>
  </Dialog>
</template>

<style scoped>
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
  padding-bottom: 1rem;
}
.w-full {
  width: 100%;
}
.name-cell,
.email-cell,
.phone-cell {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}
.font-medium {
  font-weight: 500;
}
.email-cell i,
.phone-cell i {
  color: #6c757d;
  font-size: 0.875rem;
}
.name-avatar {
  background-color: #d1fae5;
  color: #065f46;
  font-weight: bold;
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
.delete-dialog-content {
  display: flex;
  align-items: center;
  gap: 1rem;
  padding: 1.5rem 0;
}
.delete-icon {
  font-size: 2rem;
  color: #f59e0b;
}
.delete-subtitle {
  font-size: 0.875rem;
  color: #6c757d;
  margin: 0;
  margin-top: 0.25rem;
}
</style>