import _ from 'lodash'

export const EMAIL_REGEX = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/

export const WEBSITE_REGEX = /^(https?:\/\/)?([\da-z.-]+)\.([a-z.]{2,6})([/\w .-]*)*\/?$/

const minLength = min => value =>
  value && value.length < min ? `Длина должна быть больше чем ${min}` : undefined

export const required = value => (value ? undefined : 'Обязательно')

export const moreThan = (
  limit,
  message = `Должно быть больше чем ${limit}`
) => n => (n > limit ? undefined : message)

export const lessThan = (
  limit,
  message = `Должно быть меньше чем ${limit}`
) => n => (n < limit ? undefined : message)

export const integer = n =>
  n.toString().includes('.') ? 'Это не число' : undefined

export const lengthMoreThan = (
  limit,
  message = `Длина должна быть больше чем ${limit}`
) => str => (!str || str.length < limit ? message : undefined)

export const lengthLessThan = (
  limit,
  message = `Длина должна быть меньше чем ${limit}`
) => str => (!str || str.length > limit ? message : undefined)

export const minLength6 = minLength(6)

export const minLenght4 = minLength(4)

export const lengthLessThan40 = lengthLessThan(40)

export const email = value =>
  !value || !EMAIL_REGEX.test(value) ? 'Неверный адрес электронной почты' : undefined

export const uniqueAmong = (values, message = 'Должен быть уникальным') => v =>
  v && _.includes(values, v) ? message : undefined

export const website = value =>
  value && !WEBSITE_REGEX.test(value) ? 'Неверный веб-сайт' : undefined
