import type { Routes } from '@angular/router'
import { BlankComponent } from './pages/blank/blank.component'
import { ClientsComponent } from './pages/clients/clients.component'
import { locationsResolver } from './pages/clients/clients.resolver'

export const routes: Routes = [
	{ path: '', pathMatch: 'full', redirectTo: '/clients' },
	{
		path: 'clients',
		component: ClientsComponent,
		resolve: {
			clients: locationsResolver
		}
	},
	{ path: 'blank', component: BlankComponent }
]
