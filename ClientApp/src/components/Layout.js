import React, { Component } from 'react';
import { Container } from 'reactstrap';
import { NavMenu } from './NavMenu';
import {Chatroom} from "./Chatroom";
import Col from "reactstrap/es/Col";
import Row from "reactstrap/es/Row";

export class Layout extends Component {
  static displayName = Layout.name;

  render () {
    return (
      <div>
        <NavMenu />
        <Container>
            <Row>
                <Col xs={12} md={3}>
                    <Chatroom/>
                </Col>
                <Col xs={12} md={9}>
                    {this.props.children}
                </Col>
            </Row>
        </Container>
      </div>
    );
  }
}
