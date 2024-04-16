import { HttpClient, HttpParams } from '@angular/common/http'
import { Injectable, inject } from '@angular/core'
import type { Observable } from 'rxjs'
import * as z from 'zod'
import { environment } from '../../../environments/environment'

const getAllClientsSchema = z.object({
	clients: z.array(
		z.object({
			id: z.string(),
			name: z.string().max(100),
			email: z.string().email().max(100),
			birthDate: z.date()
		})
	),
	totalRows: z.number()
})

export const getAllClientsRequestSchema = z.object({
	filter: z.string().optional(),
	orderBy: z.string().optional(),
	orderDirection: z.string().optional(),
	pageSize: z.number(),
	pageNumber: z.number()
})

export type GetAllClientsRequest = z.infer<typeof getAllClientsRequestSchema>
export type GetAllClientsResponse = z.infer<typeof getAllClientsSchema>

@Injectable({ providedIn: 'root' })
export class ClientsService {
	private readonly http = inject(HttpClient)

	getClients(request: GetAllClientsRequest): Observable<GetAllClientsResponse> {
		const params = getAllClientsRequestSchema.parse(request)

		return this.http.get<GetAllClientsResponse>(`${environment.backendUrl}/clients/getAllClients`, {
			params: createHttpParams(params)
		})
	}
}

function createHttpParams<
	T extends
		| Record<string, string | number | boolean | readonly (string | number | boolean)[]>
		| undefined
>(obj: T): HttpParams {
	let params: T = {} as T
	if (obj) {
		params = Object.fromEntries(Object.entries(obj).filter(([, v]) => !!v)) as T
	}
	return new HttpParams({
		fromObject: params ?? {}
	})
}
