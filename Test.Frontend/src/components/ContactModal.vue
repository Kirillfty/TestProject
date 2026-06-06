<script setup lang="ts">
import { watch } from 'vue'
import type { Contact, ContactFormData } from '@/types/contact'
import { useContactForm } from '@/composables/useContactForm'
import PhoneInput from '@/components/PhoneInput.vue'

const props = defineProps<{
  visible: boolean
  contact: Contact | null
}>()

const emit = defineEmits<{
  saved: [data: ContactFormData]
  close: []
}>()

const { form, errors, validate, reset } = useContactForm()

watch(
  () => props.visible,
  (visible) => {
    if (visible) reset(props.contact ?? undefined)
  },
)

function handleSave() {
  if (!validate()) return
  emit('saved', { ...form })
}

function handleOverlayClick(e: MouseEvent) {
  if (e.target === e.currentTarget) emit('close')
}
</script>

<template>
  <Teleport to="body">
    <Transition name="modal">
      <div v-if="visible" class="overlay" @click="handleOverlayClick">
        <div class="modal" role="dialog" aria-modal="true">
          <div class="modal-header">
            <h2>{{ contact ? 'Редактировать контакт' : 'Добавить контакт' }}</h2>
            <button class="close-btn" aria-label="Закрыть" @click="emit('close')">×</button>
          </div>

          <div class="modal-body">
            <div class="field">
              <label for="f-name">Имя <span class="req">*</span></label>
              <input
                id="f-name"
                v-model="form.name"
                type="text"
                placeholder="Иван Иванов"
                :class="{ invalid: errors.name }"
              />
              <span v-if="errors.name" class="err">{{ errors.name }}</span>
            </div>

            <div class="field">
              <label>Мобильный телефон <span class="req">*</span></label>
              <PhoneInput v-model="form.phoneNumber" :invalid="!!errors.phoneNumber" />
              <span v-if="errors.phoneNumber" class="err">{{ errors.phoneNumber }}</span>
            </div>

            <div class="field">
              <label for="f-job">Должность</label>
              <input
                id="f-job"
                v-model="form.jobTitle"
                type="text"
                placeholder="Менеджер"
                :class="{ invalid: errors.jobTitle }"
              />
              <span v-if="errors.jobTitle" class="err">{{ errors.jobTitle }}</span>
            </div>

            <div class="field">
              <label for="f-birth">Дата рождения <span class="req">*</span></label>
              <input
                id="f-birth"
                v-model="form.birthDate"
                type="date"
                :class="{ invalid: errors.birthDate }"
              />
              <span v-if="errors.birthDate" class="err">{{ errors.birthDate }}</span>
            </div>
          </div>

          <div class="modal-footer">
            <button class="btn-secondary" @click="emit('close')">Отмена</button>
            <button class="btn-primary" @click="handleSave">Сохранить</button>
          </div>
        </div>
      </div>
    </Transition>
  </Teleport>
</template>

<style scoped>
.overlay {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, 0.45);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
}

.modal {
  background: #fff;
  border-radius: 12px;
  width: 100%;
  max-width: 480px;
  margin: 16px;
  box-shadow: 0 20px 60px rgba(0, 0, 0, 0.18);
}

.modal-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 20px 24px 16px;
  border-bottom: 1px solid #e5e7eb;
}

.modal-header h2 {
  margin: 0;
  font-size: 18px;
  font-weight: 600;
  color: #111827;
}

.close-btn {
  background: none;
  border: none;
  font-size: 24px;
  line-height: 1;
  cursor: pointer;
  color: #6b7280;
  padding: 0 2px;
}

.close-btn:hover {
  color: #111827;
}

.modal-body {
  padding: 20px 24px;
  display: flex;
  flex-direction: column;
  gap: 16px;
}

.field {
  display: flex;
  flex-direction: column;
  gap: 4px;
}

label {
  font-size: 14px;
  font-weight: 500;
  color: #374151;
}

.req {
  color: #dc2626;
}

input {
  padding: 10px 12px;
  border: 1.5px solid #d1d5db;
  border-radius: 8px;
  font-size: 14px;
  color: #111827;
  outline: none;
  transition: border-color 0.15s;
}

input:focus {
  border-color: #2563eb;
}

input.invalid {
  border-color: #dc2626;
}

.err {
  font-size: 12px;
  color: #dc2626;
}

.modal-footer {
  display: flex;
  justify-content: flex-end;
  gap: 12px;
  padding: 16px 24px 20px;
  border-top: 1px solid #e5e7eb;
}

button {
  padding: 10px 20px;
  border: none;
  border-radius: 8px;
  font-size: 14px;
  font-weight: 500;
  cursor: pointer;
  transition: background 0.15s;
}

.btn-primary {
  background: #2563eb;
  color: #fff;
}

.btn-primary:hover {
  background: #1d4ed8;
}

.btn-secondary {
  background: #f3f4f6;
  color: #374151;
}

.btn-secondary:hover {
  background: #e5e7eb;
}

.modal-enter-active,
.modal-leave-active {
  transition: opacity 0.25s cubic-bezier(0.19, 1, 0.22, 1);
}

.modal-enter-active .modal,
.modal-leave-active .modal {
  transition: transform 0.25s cubic-bezier(0.19, 1, 0.22, 1);
}

.modal-enter-from,
.modal-leave-to {
  opacity: 0;
}

.modal-enter-from .modal {
  transform: translateY(20px);
}

.modal-leave-to .modal {
  transform: translateY(20px);
}
</style>
