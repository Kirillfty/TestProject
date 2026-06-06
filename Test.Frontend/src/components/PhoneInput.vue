<script setup lang="ts">
import { ref, computed, watch, onMounted, onBeforeUnmount } from 'vue'
import { COUNTRIES, findCountry } from '@/data/countries'
import type { Country } from '@/data/countries'

const props = defineProps<{
  modelValue: string
  invalid?: boolean
}>()

const emit = defineEmits<{
  'update:modelValue': [value: string]
}>()

const selected = ref<Country>(COUNTRIES[0])
const numberPart = ref('')
const dropdownOpen = ref(false)
const rootEl = ref<HTMLElement | null>(null)

function parse(val: string) {
  const combined = selected.value.code + numberPart.value
  if (val === combined) return

  if (!val) {
    numberPart.value = ''
    return
  }

  if (/^8\d{10}$/.test(val)) {
    selected.value = COUNTRIES[0]
    numberPart.value = val.slice(1)
    return
  }

  const country = findCountry(val)
  if (country) {
    selected.value = country
    numberPart.value = val.slice(country.code.length)
  } else {
    numberPart.value = val
  }
}

watch(() => props.modelValue, parse)
onMounted(() => parse(props.modelValue))

function selectCountry(c: Country) {
  selected.value = c
  dropdownOpen.value = false
  emitValue()
}

function onInput(e: Event) {
  const raw = (e.target as HTMLInputElement).value.replace(/\D/g, '')
  numberPart.value = raw.slice(0, selected.value.digits)
  ;(e.target as HTMLInputElement).value = numberPart.value
  emitValue()
}

function emitValue() {
  emit('update:modelValue', selected.value.code + numberPart.value)
}

function onDocClick(e: MouseEvent) {
  if (rootEl.value && !rootEl.value.contains(e.target as Node)) {
    dropdownOpen.value = false
  }
}
onMounted(() => document.addEventListener('mousedown', onDocClick))
onBeforeUnmount(() => document.removeEventListener('mousedown', onDocClick))

const placeholder = computed(() => '0'.repeat(selected.value.digits))
</script>

<template>
  <div ref="rootEl" class="phone-input" :class="{ invalid }">
    <!-- Country code button -->
    <button
      type="button"
      class="code-btn"
      :class="{ open: dropdownOpen }"
      @click="dropdownOpen = !dropdownOpen"
    >
      <span class="flag">{{ selected.flag }}</span>
      <span class="code">{{ selected.code }}</span>
      <svg class="chevron" viewBox="0 0 20 20" fill="currentColor" width="14" height="14">
        <path
          fill-rule="evenodd"
          d="M5.23 7.21a.75.75 0 011.06.02L10 11.17l3.71-3.94a.75.75 0 111.08 1.04l-4.25 4.5a.75.75 0 01-1.08 0l-4.25-4.5a.75.75 0 01.02-1.06z"
          clip-rule="evenodd"
        />
      </svg>
    </button>

    <div class="divider" />

    <!-- Number input -->
    <input
      :value="numberPart"
      type="text"
      inputmode="numeric"
      :placeholder="placeholder"
      class="number-input"
      @input="onInput"
    />

    <!-- Dropdown -->
    <Transition name="dd">
      <ul v-if="dropdownOpen" class="dropdown" role="listbox">
        <li
          v-for="c in COUNTRIES"
          :key="c.name"
          class="dd-item"
          :class="{ active: c.code === selected.code && c.name === selected.name }"
          role="option"
          @mousedown.prevent="selectCountry(c)"
        >
          <span class="flag">{{ c.flag }}</span>
          <span class="dd-name">{{ c.name }}</span>
          <span class="dd-code">{{ c.code }}</span>
        </li>
      </ul>
    </Transition>
  </div>
</template>

<style scoped>
.phone-input {
  position: relative;
  display: flex;
  align-items: center;
  border: 1.5px solid #d1d5db;
  border-radius: 8px;
  transition: border-color 0.15s;
  background: #fff;
}

.phone-input:focus-within {
  border-color: #2563eb;
}

.phone-input.invalid {
  border-color: #dc2626;
}

.code-btn {
  display: flex;
  align-items: center;
  gap: 4px;
  padding: 0 10px;
  height: 42px;
  background: none;
  border: none;
  cursor: pointer;
  color: #374151;
  font-size: 14px;
  white-space: nowrap;
  flex-shrink: 0;
}

.code-btn:hover {
  background: #f3f4f6;
  border-radius: 7px 0 0 7px;
}

.flag {
  font-size: 18px;
  line-height: 1;
}

.code {
  font-weight: 500;
}

.chevron {
  color: #6b7280;
  transition: transform 0.2s;
}

.code-btn.open .chevron {
  transform: rotate(180deg);
}

.divider {
  width: 1px;
  height: 22px;
  background: #d1d5db;
  flex-shrink: 0;
}

.number-input {
  flex: 1;
  padding: 10px 12px;
  border: none;
  outline: none;
  font-size: 14px;
  color: #111827;
  background: transparent;
  min-width: 0;
}

.number-input::placeholder {
  color: #9ca3af;
  letter-spacing: 0.05em;
}

.dropdown {
  position: absolute;
  top: calc(100% + 4px);
  left: 0;
  z-index: 200;
  background: #fff;
  border: 1px solid #e5e7eb;
  border-radius: 10px;
  box-shadow: 0 8px 24px rgba(0, 0, 0, 0.12);
  list-style: none;
  margin: 0;
  padding: 4px;
  min-width: 220px;
  max-height: 280px;
  overflow-y: auto;
}

.dd-item {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 8px 10px;
  border-radius: 6px;
  cursor: pointer;
  font-size: 14px;
}

.dd-item:hover,
.dd-item.active {
  background: #eff6ff;
}

.dd-name {
  flex: 1;
  color: #111827;
}

.dd-code {
  color: #6b7280;
  font-size: 13px;
  font-variant-numeric: tabular-nums;
}

.dd-enter-active,
.dd-leave-active {
  transition:
    opacity 0.15s ease,
    transform 0.15s ease;
}

.dd-enter-from,
.dd-leave-to {
  opacity: 0;
  transform: translateY(-6px);
}
</style>
