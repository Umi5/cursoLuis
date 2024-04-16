import { inject } from '@angular/core'
import type { ActivatedRouteSnapshot, ResolveFn, RouterStateSnapshot } from '@angular/router'
import { NzMessageService } from 'ng-zorro-antd/message'
import { NzNotificationService } from 'ng-zorro-antd/notification'
import { EMPTY, catchError, delay, finalize } from 'rxjs'
import { ClientsService, type GetAllClientsResponse } from './clients.service'

export const locationsResolver: ResolveFn<GetAllClientsResponse> = (
	route: ActivatedRouteSnapshot,
	state: RouterStateSnapshot
) => {
	const clientService = inject(ClientsService)
	const messageService = inject(NzMessageService)
	const notificationService = inject(NzNotificationService)

	const defaultFilter = {
		filter: '',
		orderBy: 'name',
		orderDirection: 'asc',
		pageSize: 6,
		pageNumber: 1
	}

	const messageId = messageService.loading('Cargando informacion de clientes....', {
		nzDuration: 0
	}).messageId

	return clientService.getClients(defaultFilter).pipe(
		delay(1000),
		finalize(() => messageService.remove(messageId)),
		catchError((error) => {
			console.error('Error loading clients', error)
			messageService.remove(messageId)

			notificationService.error(
				'Error al cargar clientes',
				'Ha habido un error al cargar los clientes. Por favor, intenta de nuevo.'
			)

			return EMPTY
		})
	)
}
