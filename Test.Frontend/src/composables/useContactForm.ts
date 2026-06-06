import { reactive } from 'vue'
import type { Contact, ContactFormData } from '@/types/contact'
import { findCountry } from '@/data/countries'

const NAME_RE = /^[A-Za-zА-Яа-яЁё\s-]+$/

export function useContactForm() {
  const form = reactive<ContactFormData>({
    name: '',
    phoneNumber: '',
    jobTitle: '',
    birthDate: '',
  })

  const errors = reactive<Record<keyof ContactFormData, string>>({
    name: '',
    phoneNumber: '',
    jobTitle: '',
    birthDate: '',
  })

  function validate(): boolean {
    errors.name = ''
    errors.phoneNumber = ''
    errors.jobTitle = ''
    errors.birthDate = ''

    let valid = true

    if (!form.name.trim()) {
      errors.name = 'Имя обязательно'
      valid = false
    } else if (form.name.length < 2 || form.name.length > 100) {
      errors.name = 'Длина имени: 2–100 символов'
      valid = false
    } else if (!NAME_RE.test(form.name)) {
      errors.name = 'Только буквы, пробелы и дефис'
      valid = false
    }

    if (!form.phoneNumber) {
      errors.phoneNumber = 'Телефон обязателен'
      valid = false
    } else {
      const country = findCountry(form.phoneNumber)
      if (!country) {
        errors.phoneNumber = 'Выберите код страны'
        valid = false
      } else {
        const digits = form.phoneNumber.slice(country.code.length)
        if (!/^\d+$/.test(digits) || digits.length !== country.digits) {
          errors.phoneNumber = `Введите ${country.digits} цифр номера`
          valid = false
        }
      }
    }

    if (form.jobTitle && form.jobTitle.length > 100) {
      errors.jobTitle = 'Не более 100 символов'
      valid = false
    }

    if (!form.birthDate) {
      errors.birthDate = 'Дата рождения обязательна'
      valid = false
    } else {
      const birth = new Date(form.birthDate)
      const today = new Date()
      today.setHours(0, 0, 0, 0)

      if (birth > today) {
        errors.birthDate = 'Дата не может быть в будущем'
        valid = false
      } else {
        const age = calcAge(birth, today)
        if (age < 14) {
          errors.birthDate = 'Минимальный возраст: 14 лет'
          valid = false
        } else if (age > 120) {
          errors.birthDate = 'Максимальный возраст: 120 лет'
          valid = false
        }
      }
    }

    return valid
  }

  function reset(contact?: Contact) {
    errors.name = ''
    errors.phoneNumber = ''
    errors.jobTitle = ''
    errors.birthDate = ''

    if (contact) {
      form.name = contact.name
      const ph = contact.phoneNumber ?? ''
      form.phoneNumber = /^8\d{10}$/.test(ph) ? '+7' + ph.slice(1) : ph
      form.jobTitle = contact.jobTitle ?? ''
      form.birthDate = contact.birthDate ? contact.birthDate.split('T')[0] : ''
    } else {
      form.name = ''
      form.phoneNumber = ''
      form.jobTitle = ''
      form.birthDate = ''
    }
  }

  return { form, errors, validate, reset }
}

function calcAge(birth: Date, today: Date): number {
  let age = today.getFullYear() - birth.getFullYear()
  const m = today.getMonth() - birth.getMonth()
  if (m < 0 || (m === 0 && today.getDate() < birth.getDate())) age--
  return age
}
