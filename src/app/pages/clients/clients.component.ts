import { CommonModule, DatePipe } from '@angular/common'
import {
	ChangeDetectionStrategy,
	Component,
	type OnInit,
	type WritableSignal,
	inject,
	signal
} from '@angular/core'
import { FormsModule } from '@angular/forms'
import { ActivatedRoute } from '@angular/router'
import { NzButtonModule } from 'ng-zorro-antd/button'
import { NzIconModule } from 'ng-zorro-antd/icon'
import { NzInputModule } from 'ng-zorro-antd/input'
import { NzTableModule, type NzTableQueryParams } from 'ng-zorro-antd/table'
import { Subject, catchError, debounceTime, delay, finalize } from 'rxjs'
import { ClientsService, type GetAllClientsResponse } from './clients.service'

type Client = GetAllClientsResponse['clients'][0]

@Component({
	selector: 'app-clients',
	standalone: true,
	imports: [
		CommonModule,
		FormsModule,
		NzTableModule,
		NzInputModule,
		NzIconModule,
		NzButtonModule,
		DatePipe
	],
	templateUrl: './clients.component.html',
	styles: '',
	changeDetection: ChangeDetectionStrategy.OnPush
})
export class ClientsComponent implements OnInit {
	private readonly activartedRoute = inject(ActivatedRoute)
	private readonly clientService = inject(ClientsService)

	private searchSubject = new Subject<string>()

	private firstLoad = false

	filterInfo: WritableSignal<{
		pageSize: number
		pageIndex: number
		sortField?: string
		sortOrder?: string
	}> = signal({
		pageSize: 6,
		pageIndex: 1,
		sortField: 'name',
		sortOrder: 'asc'
	})

	clients: Client[] = []
	searchText = ''
	loading = signal(false)
	totalRows = signal(0)

	ngOnInit() {
		const data = this.activartedRoute.snapshot.data['clients'] as GetAllClientsResponse

		this.clients = data.clients
		this.totalRows.set(data.totalRows)

		this.searchSubject.pipe(debounceTime(350)).subscribe((searchText) => {
			this.loading.set(true)
			this.clientService
				.getClients({
					filter: this.searchText,
					orderBy: this.filterInfo().sortField,
					orderDirection: this.filterInfo().sortOrder === 'ascend' ? 'asc' : 'desc',
					pageSize: this.filterInfo().pageSize,
					pageNumber: this.filterInfo().pageIndex
				})
				.pipe(
					delay(1000),
					finalize(() => {
						this.loading.set(false)
					})
				)
				.subscribe({
					next: (response) => {
						this.clients = response.clients
						this.totalRows.set(response.totalRows)
					},
					error: (error) => {
						alert('Error loading clients')
					}
				})
		})
	}

	onQueryParamsChange(params: NzTableQueryParams) {
		if (!this.firstLoad) {
			this.firstLoad = true
			return
		}

		const { pageSize, pageIndex, sort } = params
		const currentSort = sort.find((item) => item.value !== null)
		const sortField = currentSort?.key || undefined
		const sortOrder = currentSort?.value || undefined

		this.filterInfo.set({
			pageSize,
			pageIndex,
			sortField,
			sortOrder
		})

		this.loading.set(true)

		this.clientService
			.getClients({
				filter: this.searchText,
				orderBy: this.filterInfo().sortField,
				orderDirection: this.filterInfo().sortOrder === 'ascend' ? 'asc' : 'desc',
				pageSize: this.filterInfo().pageSize,
				pageNumber: this.filterInfo().pageIndex
			})
			.pipe(
				delay(1000),
				finalize(() => {
					this.loading.set(false)
				})
			)
			.subscribe({
				next: (response) => {
					this.clients = response.clients
					this.totalRows.set(response.totalRows)
				},
				error: (error) => {
					alert('Error loading clients')
				}
			})
	}

	onSearch($event: Event) {
		this.searchSubject.next(this.searchText)
	}
}
