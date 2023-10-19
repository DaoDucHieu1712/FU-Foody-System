import axios from 'axios';

const instance = axios.create({
  baseURL: 'https://localhost:7025',
});

export default instance;    