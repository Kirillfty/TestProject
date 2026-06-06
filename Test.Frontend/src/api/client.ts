import axios from 'axios'

const client = axios.create({
  baseURL: '/Contact',
  headers: { 'Content-Type': 'application/json' },
})

export default client
