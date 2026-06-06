<script setup lang="ts">
import { reactive, onMounted } from 'vue'
import type { Contact, ContactFormData } from '@/types/contact'
import { useContacts } from '@/composables/useContacts'
import ContactsTable from '@/components/ContactsTable.vue'
import ContactModal from '@/components/ContactModal.vue'
import ConfirmModal from '@/components/ConfirmModal.vue'

const { contacts, loading, error, fetchContacts, createContact, updateContact, deleteContact } =
  useContacts()

const modal = reactive<{
  visible: boolean
  mode: 'create' | 'edit'
  contact: Contact | null
}>({
  visible: false,
  mode: 'create',
  contact: null,
})

const confirmModal = reactive<{
  visible: boolean
  contact: Contact | null
}>({
  visible: false,
  contact: null,
})

onMounted(fetchContacts)

function openCreate() {
  modal.mode = 'create'
  modal.contact = null
  modal.visible = true
}

function openEdit(contact: Contact) {
  modal.mode = 'edit'
  modal.contact = contact
  modal.visible = true
}

function closeModal() {
  modal.visible = false
}

async function onSaved(data: ContactFormData) {
  let ok: boolean
  if (modal.mode === 'edit' && modal.contact) {
    ok = await updateContact(modal.contact.id, data)
  } else {
    ok = await createContact(data)
  }
  if (ok) modal.visible = false
}

function onDelete(contact: Contact) {
  confirmModal.contact = contact
  confirmModal.visible = true
}

async function onDeleteConfirmed() {
  if (confirmModal.contact) await deleteContact(confirmModal.contact.id)
  confirmModal.visible = false
  confirmModal.contact = null
}

function onDeleteCancelled() {
  confirmModal.visible = false
  confirmModal.contact = null
}
</script>

<template>
  <div class="page">
    <div class="page-header">
      <h1>Контакты</h1>
      <button class="btn-add" @click="openCreate">+ Добавить контакт</button>
    </div>

    <div v-if="error" class="error-banner">{{ error }}</div>

    <div v-if="loading" class="loader">
      <div class="spinner" />
      <span>Загрузка...</span>
    </div>

    <ContactsTable v-else :contacts="contacts" @edit="openEdit" @delete="onDelete" />

    <ContactModal
      :visible="modal.visible"
      :contact="modal.contact"
      @saved="onSaved"
      @close="closeModal"
    />

    <ConfirmModal
      :visible="confirmModal.visible"
      :message="`Вы уверены, что хотите удалить контакт «${confirmModal.contact?.name}»?`"
      @confirm="onDeleteConfirmed"
      @cancel="onDeleteCancelled"
    />
  </div>
</template>

<style scoped>
.page {
  max-width: 960px;
  margin: 0 auto;
  padding: 40px 24px;
}

.page-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 24px;
}

.page-header h1 {
  margin: 0;
  font-size: 24px;
  font-weight: 700;
  color: #111827;
}

.btn-add {
  padding: 10px 20px;
  background: #2563eb;
  color: #fff;
  border: none;
  border-radius: 8px;
  font-size: 14px;
  font-weight: 600;
  cursor: pointer;
  transition: background 0.15s;
}

.btn-add:hover {
  background: #1d4ed8;
}

.error-banner {
  background: #fef2f2;
  border: 1px solid #fecaca;
  color: #dc2626;
  border-radius: 8px;
  padding: 12px 16px;
  margin-bottom: 20px;
  font-size: 14px;
}

.loader {
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 48px 0;
  justify-content: center;
  color: #6b7280;
  font-size: 14px;
}

.spinner {
  width: 24px;
  height: 24px;
  border: 3px solid #e5e7eb;
  border-top-color: #2563eb;
  border-radius: 50%;
  animation: spin 0.7s linear infinite;
}

@keyframes spin {
  to {
    transform: rotate(360deg);
  }
}
</style>
