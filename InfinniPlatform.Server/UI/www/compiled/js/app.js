//####.js\routerCallbacks.js
function openHomePage(context, args) {
	if( context.controls["MainContent"] != null ) {
		context.controls["MainContent"].setLayout(null);
	}
};

function openDatagridPage(context, args) {
	context.global.executeAction(context, {
		OpenAction: {
			LinkView: {
				AutoView: {
					Path: "viewExample/dataGrid.json",
					OpenMode: "Container",
					Container: "MainContent"
				}
			}
		}
	});
};

function openLoginPage(context, args) {
	context.global.executeAction(context, {
		OpenAction: {
			LinkView: {
				AutoView: {
					Path: "viewExample/loginPage.json",
					OpenMode: "Container",
					Container: "MainContent"
				}
			}
		}
	});
};

function openDataBindingPage(context, args) {
	var itemId = args.params[0];

	context.global.executeAction(context, {
		OpenAction: {
			LinkView: {
				AutoView: {
					Path: "viewExample/binding.json",
					OpenMode: "Container",
					Container: "MainContent"
				}
			}
		}
	});
};


//####.js\extentionPanels\testExtention.js
function TestExtension(context, args) {
    this.context = args.context;

    this.$el = args.$el;
    this.parameters = args.parameters;
    this.itemTemplate = args.itemTemplate;
}

_.extend( TestExtension.prototype, {
    render: function(){
        this.$el
            .append('<div>TestExtension</div> ')
            .append(this.itemTemplate(this.context, {index:0}).render());
    }
});
//####.js\elements\testElement\testControl.js
var TestControl = function (viewMode) {
    _.superClass(TestControl, this, viewMode);
};

_.inherit(TestControl, InfinniUI.Control);

_.extend(TestControl.prototype, {

    createControlModel: function () {
        return new TestModel();
    },

    createControlView: function (model, viewMode) {
        return new TestView({model: model});
    }

});
//####.js\elements\testElement\testElement.js
function TestElement(parent, viewMode) {
    _.superClass(TestElement, this, parent, viewMode);
}

_.inherit(TestElement, InfinniUI.Element);


_.extend(TestElement.prototype, {

        createControl: function () {
            return new TestControl();
        },

        setTestProperty: function (value) {
            this.control.set('testProperty', value);
        },

        getTestProperty: function () {
            return this.control.get('testProperty');
        }

    }
);
//####.js\elements\testElement\testElementBuilder.js
function TestElementBuilder() {
    _.superClass(TestElementBuilder, this);
}

_.inherit(TestElementBuilder, InfinniUI.ElementBuilder);

_.extend(TestElementBuilder.prototype, {
        applyMetadata: function(params){
            var element = params.element;
            var metadata = params.metadata;

            InfinniUI.ElementBuilder.prototype.applyMetadata.call(this, params);

            if('TestProperty' in metadata){
                element.setTestProperty(metadata['TestProperty']);
            }
        },

        createElement: function(params){
            return new TestElement(params.parent, params.metadata['ViewMode']);
        }

    }
);

InfinniUI.ApplicationBuilder.addToRegisterQueue('TestElement', new TestElementBuilder());

//####.js\elements\testElement\testModel.js
var TestModel = InfinniUI.ControlModel.extend(_.extend({

    defaults: _.defaults({
        testProperty: 'testPropertyValue'
    }, InfinniUI.ControlModel.prototype.defaults),

    initialize: function(){
        InfinniUI.ControlModel.prototype.initialize.apply(this, arguments);
    }
}));
//####.js\elements\testElement\testView.js
var TestView = InfinniUI.ControlView.extend({
    tagName: 'div',
    className: 'pl-test-view',

    template: InfinniUI.Template["elements/testElement/template/testElement.tpl.html"],

    UI: {
        control: '.pl-test-element-in'
    },


    initialize: function () {
        InfinniUI.ControlView.prototype.initialize.apply(this);
    },

    initHandlersForProperties: function(){
        InfinniUI.ControlView.prototype.initHandlersForProperties.call(this);

        this.listenTo(this.model, 'change:testProperty', this.updateTestProperty);
    },

    updateProperties: function(){
        InfinniUI.ControlView.prototype.updateProperties.call(this);

        this.updateTestProperty();
    },

    updateTestProperty: function(){
        var testProperty = this.model.get('testProperty');
        this.ui.control.html(testProperty);
    },

    render: function () {
        this.prerenderingActions();
        this.renderTemplate(this.template);

        this.updateProperties();

        this.trigger('render');
        this.postrenderingActions();
        return this;
    }

});
