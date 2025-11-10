import axios from 'axios';

const apiClient = axios.create({
    baseURL: 'http://localhost:5239/api',
    headers:{
        'Content-Type': 'application/json'
    }
})

export default {
    getContatos(){
        return apiClient.get('/Contatos');
    },
    getContato(id){
        return apiClient.get(`/Contatos/${id}`);
    },
    createContato(contatoDto){
        return apiClient.post('/Contatos', contatoDto);
    },
    updateContato(id, contatoDto){
        return apiClient.put(`/Contatos/${id}`, contatoDto);
    },
    deleteContato(id){
        return apiClient.delete(`/Contatos/${id}`);
    }
}