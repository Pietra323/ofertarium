import { createApp } from 'vue'
import './style.css'
import App from './App.vue'
import axios from "axios";

const jwtToken = localStorage.getItem('jwtToken');
if (jwtToken) {
    axios.defaults.headers.common['Authorization'] = `Bearer ${jwtToken}`;
}

createApp(App).mount('#app')
