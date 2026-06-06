<script setup lang="ts">
defineProps<{
  visible: boolean
  message: string
}>()

const emit = defineEmits<{
  confirm: []
  cancel: []
}>()

function handleOverlayClick(e: MouseEvent) {
  if (e.target === e.currentTarget) emit('cancel')
}
</script>

<template>
  <Teleport to="body">
    <Transition name="confirm">
      <div v-if="visible" class="overlay" @click="handleOverlayClick">
        <div class="modal" role="alertdialog" aria-modal="true">
          <div class="icon">
            <svg viewBox="0 0 24 24" fill="none" width="28" height="28">
              <circle cx="12" cy="12" r="10" fill="#fef2f2" stroke="#fecaca" stroke-width="1.5" />
              <path
                d="M12 8v4m0 4h.01"
                stroke="#dc2626"
                stroke-width="2"
                stroke-linecap="round"
                stroke-linejoin="round"
              />
            </svg>
          </div>
          <p class="message">{{ message }}</p>
          <div class="footer">
            <button class="btn-cancel" @click="emit('cancel')">Отмена</button>
            <button class="btn-confirm" @click="emit('confirm')">Удалить</button>
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
  max-width: 360px;
  margin: 16px;
  padding: 28px 24px 24px;
  box-shadow: 0 20px 60px rgba(0, 0, 0, 0.18);
  display: flex;
  flex-direction: column;
  align-items: center;
  text-align: center;
  gap: 16px;
}

.message {
  margin: 0;
  font-size: 15px;
  color: #111827;
  line-height: 1.5;
}

.footer {
  display: flex;
  gap: 12px;
  width: 100%;
}

button {
  flex: 1;
  padding: 10px;
  border: none;
  border-radius: 8px;
  font-size: 14px;
  font-weight: 500;
  cursor: pointer;
  transition: background 0.15s;
}

.btn-cancel {
  background: #f3f4f6;
  color: #374151;
}

.btn-cancel:hover {
  background: #e5e7eb;
}

.btn-confirm {
  background: #dc2626;
  color: #fff;
}

.btn-confirm:hover {
  background: #b91c1c;
}

.confirm-enter-active,
.confirm-leave-active {
  transition: opacity 0.2s ease;
}

.confirm-enter-active .modal,
.confirm-leave-active .modal {
  transition: transform 0.2s ease;
}

.confirm-enter-from,
.confirm-leave-to {
  opacity: 0;
}

.confirm-enter-from .modal {
  transform: scale(0.95);
}

.confirm-leave-to .modal {
  transform: scale(0.95);
}
</style>
