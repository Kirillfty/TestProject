export interface Country {
  code: string
  flag: string
  name: string
  digits: number
}

export const COUNTRIES: Country[] = [
  { code: '+7',   flag: '🇷🇺', name: 'Россия',         digits: 10 },
  { code: '+375', flag: '🇧🇾', name: 'Беларусь',       digits: 9  },
  { code: '+380', flag: '🇺🇦', name: 'Украина',        digits: 9  },
  { code: '+998', flag: '🇺🇿', name: 'Узбекистан',     digits: 9  },
  { code: '+7',   flag: '🇰🇿', name: 'Казахстан',      digits: 10 },
  { code: '+1',   flag: '🇺🇸', name: 'США / Канада',   digits: 10 },
  { code: '+44',  flag: '🇬🇧', name: 'Великобритания', digits: 10 },
  { code: '+49',  flag: '🇩🇪', name: 'Германия',       digits: 10 },
  { code: '+33',  flag: '🇫🇷', name: 'Франция',        digits: 9  },
  { code: '+48',  flag: '🇵🇱', name: 'Польша',         digits: 9  },
]

export function findCountry(phone: string): Country | undefined {
  const sorted = [...COUNTRIES].sort((a, b) => b.code.length - a.code.length)
  return sorted.find((c) => phone.startsWith(c.code))
}
