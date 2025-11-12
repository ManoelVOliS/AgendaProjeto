import axios from 'axios';

const apiClient = axios.create({
    baseURL: 'http://localhost:8080/api',
    headers:{
        'Content-Type': 'application/json'
    }
})

export default {
    getContacts(){
        return apiClient.get('/Contacts');
    },
    getContact(id){
        return apiClient.get(`/Contacts/${id}`);
    },
    createContact(contactDto){
        return apiClient.post('/Contacts', contactDto);
    },
    updateContact(id, contactDto){
        return apiClient.put(`/Contacts/${id}`, contactDto);
    },
    deleteContact(id){
        return apiClient.delete(`/Contacts/${id}`);
    }
}