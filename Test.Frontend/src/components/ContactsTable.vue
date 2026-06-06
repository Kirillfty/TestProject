<script setup lang="ts">
import type { Contact } from '@/types/contact'

defineProps<{ contacts: Contact[] }>()

const emit = defineEmits<{
  edit: [contact: Contact]
  delete: [contact: Contact]
}>()

function formatDate(iso: string): string {
  if (!iso) return '—'
  return new Date(iso).toLocaleDateString('ru-RU')
}
</script>

<template>
  <div class="table-wrapper">
    <table>
      <thead>
        <tr>
          <th>Имя</th>
          <th>Мобильный телефон</th>
          <th>Должность</th>
          <th>Дата рождения</th>
          <th>Действия</th>
        </tr>
      </thead>
      <tbody>
        <tr v-if="contacts.length === 0">
          <td colspan="5" class="empty">Контакты не найдены</td>
        </tr>
        <tr v-for="contact in contacts" :key="contact.id">
          <td>{{ contact.name }}</td>
          <td>{{ contact.phoneNumber }}</td>
          <td>{{ contact.jobTitle || '—' }}</td>
          <td>{{ formatDate(contact.birthDate) }}</td>
          <td class="actions">
            <button class="btn-edit" @click="emit('edit', contact)">Редактировать</button>
            <button class="btn-delete" @click="emit('delete', contact)">Удалить</button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<style scoped>
.table-wrapper {
  overflow-x: auto;
  border-radius: 10px;
  border: 1px solid #e5e7eb;
}

table {
  width: 100%;
  border-collapse: collapse;
  font-size: 14px;
}

th,
td {
  padding: 12px 16px;
  text-align: left;
  border-bottom: 1px solid #e5e7eb;
}

tbody tr:last-child td {
  border-bottom: none;
}

th {
  background: #f9fafb;
  font-weight: 600;
  color: #374151;
  white-space: nowrap;
}

tbody tr:hover td {
  background: #f9fafb;
}

.empty {
  text-align: center;
  color: #9ca3af;
  padding: 48px 16px;
}

.actions {
  display: flex;
  gap: 8px;
  white-space: nowrap;
}

button {
  padding: 6px 12px;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  font-size: 13px;
  font-weight: 500;
  transition: background 0.15s;
}

.btn-edit {
  background: #eff6ff;
  color: #2563eb;
}

.btn-edit:hover {
  background: #dbeafe;
}

.btn-delete {
  background: #fef2f2;
  color: #dc2626;
}

.btn-delete:hover {
  background: #fee2e2;
}
</style>
