import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
    selector: 'app-blank',
    standalone: true,
    imports: [
        CommonModule,
    ],
    template: `<p>blank works!</p>`,
    styles: ``,
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class BlankComponent { }
