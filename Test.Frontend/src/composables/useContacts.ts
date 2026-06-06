import { ref } from 'vue'
import axios from 'axios'
import client from '@/api/client'
import type { Contact, ContactFormData } from '@/types/contact'

export function useContacts() {
  const contacts = ref<Contact[]>([])
  const loading = ref(false)
  const error = ref<string | null>(null)

  async function fetchContacts() {
    loading.value = true
    error.value = null
    try {
      const { data } = await client.get<Contact[]>('/')
      contacts.value = data
    } catch (e) {
      error.value = extractMessage(e, 'Не удалось загрузить контакты')
    } finally {
      loading.value = false
    }
  }

  async function createContact(data: ContactFormData): Promise<boolean> {
    error.value = null
    try {
      await client.post('/', {
        name: data.name,
        phoneNumber: data.phoneNumber,
        jobTitle: data.jobTitle,
        birthDate: data.birthDate,
      })
      await fetchContacts()
      return true
    } catch (e) {
      error.value = extractMessage(e, 'Не удалось создать контакт')
      return false
    }
  }

  async function updateContact(id: number, data: ContactFormData): Promise<boolean> {
    error.value = null
    try {
      await client.put('/', {
        id,
        name: data.name,
        phoneNumber: data.phoneNumber,
        jobTitle: data.jobTitle,
        birthDate: data.birthDate,
      })
      await fetchContacts()
      return true
    } catch (e) {
      error.value = extractMessage(e, 'Не удалось обновить контакт')
      return false
    }
  }

  async function deleteContact(id: number): Promise<boolean> {
    error.value = null
    try {
      await client.delete(`/${id}`)
      contacts.value = contacts.value.filter((c) => c.id !== id)
      return true
    } catch (e) {
      error.value = extractMessage(e, 'Не удалось удалить контакт')
      return false
    }
  }

  return { contacts, loading, error, fetchContacts, createContact, updateContact, deleteContact }
}

function extractMessage(e: unknown, fallback: string): string {
  if (axios.isAxiosError(e)) {
    return e.response?.data?.title ?? e.response?.data ?? e.message ?? fallback
  }
  return e instanceof Error ? e.message : fallback
}
